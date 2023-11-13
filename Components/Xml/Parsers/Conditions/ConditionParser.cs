using Components.DataModels;
using Components.Extensions;
using Components.Interfaces.Conditions;
using Components.Interfaces.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using System.Text;
using System.Text.RegularExpressions;
namespace Components.Xml.Parsers.Conditions;

internal class ConditionParser : IConditionParser
{
    private readonly string [] legalStateNames =
    {
        "horizontal",
    };

    public ConditionFormat ParseFormat { private get; set; }

    public ConditionParser ()
    {
        ParseFormat = new ConditionFormat ();
    }

    public ConditionParser (ConditionFormat parseFormat)
    {
        ParseFormat = parseFormat;
    }

    public IConditionTreeItem Parse (IComponentDescription description, string input)
    {
        Queue<ConditionToken> output = new Queue<ConditionToken> ();
        Stack<ConditionToken> operators = new Stack<ConditionToken> ();

        PositioningReader reader = new PositioningReader (new StringReader (input));
        ConditionToken? token = ReadToken (reader);
        while (token.HasValue)
        {
            ConditionToken t = token.Value;

            if (t.Type == ConditionToken.TokenType.Symbol)
                output.Enqueue (t);
            else if (t.Type == ConditionToken.TokenType.LeftBracket)
                operators.Push (t);
            else if (t.Type == ConditionToken.TokenType.RightBracket)
            {
                ConditionToken n = operators.Pop ();
                while (n.Type != ConditionToken.TokenType.LeftBracket)
                {
                    if (operators.Count == 0)
                        throw new Exception ("Syntax error");

                    output.Enqueue (n);
                    n = operators.Pop ();
                }
                if (operators.Count > 0 && operators.Peek () == ConditionToken.AND)
                    output.Enqueue (operators.Pop ());
            }
            else if (t == ConditionToken.AND)
            {
                while (operators.Count > 0 && (operators.Peek () == ConditionToken.AND || operators.Peek () == ConditionToken.OR))
                {
                    output.Enqueue (operators.Pop ());
                }
                operators.Push (t);
            }
            else if (t == ConditionToken.OR)
            {
                while (operators.Count > 0 &&
                       operators.Peek ().Type == ConditionToken.TokenType.Operator && operators.Peek ().Operator == ConditionToken.OperatorType.OR)
                {
                    output.Enqueue (operators.Pop ());
                }

                operators.Push (t);
            }

            token = ReadToken (reader);
        }

        while (operators.Count > 0)
        {
            output.Enqueue (operators.Pop ());
        }

        Queue<ConditionToken> reversed = new Queue<ConditionToken> (output.Reverse ());

        return ParseToken (reversed, description);
    }

    private ConditionToken? ReadToken (PositioningReader reader)
    {
        int position = reader.CharPos;

        int c = reader.Read ();
        if (c == -1)
            return null;

        switch (c)
        {
            case '|':
                return ConditionToken.OR;
            case ',':
                return ConditionToken.AND;
            case '(':
                return ConditionToken.LeftBracket;
            case ')':
                return ConditionToken.RightBracket;
            default:
                StringBuilder builder = new StringBuilder (((char)c).ToString ());
                int next = reader.Peek ();
                while (next != -1 && next != ',' && next != '|' && next != '(' && next != ')')
                {
                    builder.Append ((char)reader.Read ());
                    next = reader.Peek ();
                }

                return new ConditionToken (builder.ToString (), position);
        }
    }

    private IConditionTreeItem ParseToken (Queue<ConditionToken> r, IComponentDescription description)
    {
        if (r.Count == 0)
            throw new Exception ("Invalid condition");

        ConditionToken t = r.Dequeue ();
        if (t.Type == ConditionToken.TokenType.Symbol)
            return ParseLeaf (t, description);
        else if (t.Type == ConditionToken.TokenType.Operator && t.Operator == ConditionToken.OperatorType.AND)
        {
            IConditionTreeItem right = ParseToken (r, description);
            IConditionTreeItem left = ParseToken (r, description);

            return new ConditionTree (
                ConditionTree.ConditionOperator.AND,
                left,
                right);
        }
        else if (t.Type == ConditionToken.TokenType.Operator && t.Operator == ConditionToken.OperatorType.OR)
        {
            IConditionTreeItem right = ParseToken (r, description);
            IConditionTreeItem left = ParseToken (r, description);

            return new ConditionTree (
                ConditionTree.ConditionOperator.OR,
                left,
                right);
        }
        else
            throw new ArgumentException ("Invalid Queue", "r");
    }

    private ConditionTreeLeaf ParseLeaf (ConditionToken token, IComponentDescription description)
    {
        bool isNegated;
        bool isState;
        string propertyName;
        string comparisonStr;
        string compareToStr;
        SplitLeaf (token, out isNegated, out isState, out propertyName, out comparisonStr, out compareToStr);

        if (compareToStr == string.Empty)
            compareToStr = "true"; // Implicit true

        ConditionComparison comparison;
        switch (comparisonStr)
        {
            case "==":
                comparison = ConditionComparison.Equal;
                break;
            case "!=":
                comparison = ConditionComparison.NotEqual;
                break;
            case "[GreaterThan]":
                comparison = ConditionComparison.Greater;
                break;
            case "[LessThan]":
                comparison = ConditionComparison.Less;
                break;
            case "[GreaterOrEqual]":
                comparison = ConditionComparison.GreaterOrEqual;
                break;
            case "[LessOrEqual]":
                comparison = ConditionComparison.LessOrEqual;
                break;
            default:
                comparison = ConditionComparison.Truthy;
                break;
        }

        if (isNegated && comparison == ConditionComparison.Equal)
            comparison = ConditionComparison.NotEqual;
        else if (isNegated && comparison == ConditionComparison.Truthy)
            comparison = ConditionComparison.Falsy;
        else if (isNegated)
        {
            // Operator cannot be negated
            throw new Exception ("Comparison cannot be negated (illegal '!')");
        }

        if (isState)
        {
            if (!legalStateNames.Contains (propertyName.ToLowerInvariant ()))
                throw new Exception (string.Format ("Unknown component state '{0}'", propertyName));

            return new ConditionTreeLeaf (ConditionType.State, propertyName, comparison, PropertyValue.Parse (compareToStr, PropertyValue.Type.Boolean));
        }
        else
        {
            IComponentDescriptionProperty? property;
            if ((property = description.Properties.FirstOrDefault (x => x.Name == propertyName)) == null)
            {
                if (propertyName.StartsWith ("Show"))
                {
                    string subbedName = propertyName.Substring (4);
                    if ((property = description.Properties.FirstOrDefault (x => x.Name == subbedName)) is null)
                        throw new Exception (string.Format ("Unknown property '{0}'", subbedName));
                }
            }

            return new ConditionTreeLeaf (ConditionType.Property, propertyName, comparison, PropertyValue.Parse (compareToStr, property!.Type.ToSimplePropertyType ()));
        }
    }

    public static void SplitLeaf (ConditionToken token, out bool isNegated, out bool isState, out string property, out string op, out string compareTo)
    {
        string rIsNegated = @"(!?)";
        string rIsState = @"(\$?)";
        string rProperty = @"([a-zA-Z]+[a-zA-Z0-9_]*)";
        string rOperator = @"((==|\[GreaterThan\]|\[LessThan\]|\[LessOrEqual\]|\[GreaterOrEqual\]|!=)";
        string rCompareTo = @"([a-zA-Z0-9_.]+))?";

        Regex regex = new Regex (string.Format ("{0}{1}{2}{3}{4}", rIsNegated, rIsState, rProperty, rOperator, rCompareTo));
        Match match = regex.Match (token.Symbol);

        if (!match.Success)
            throw new ArgumentException ("Invalid syntax");

        isNegated = match.Groups [1].Value.Length == 1;
        isState = match.Groups [2].Value.Length == 0;
        property = match.Groups [3].Value;
        op = match.Groups [5].Value;
        compareTo = match.Groups [6].Value;
    }
}
