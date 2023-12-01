using Components.Base.DataModels;
using Components.Base.Models;
using Components.Base.Primitives;
using Components.Render.TypeDescription.Extensions;
namespace Components.Render.TypeDescription.Conditions;

public class ConditionTreeLeaf : IConditionTreeItem
{
    public ConditionType Type { get; }
    public ConditionComparison Comparison { get; }
    public string VariableName { get; }
    public PropertyValue CompareTo { get; }

    public ConditionTreeLeaf ()
    {
        Type = ConditionType.Empty;
        Comparison = ConditionComparison.Falsy;
        VariableName = string.Empty;
        CompareTo = new PropertyValue ();
    }

    public ConditionTreeLeaf (ConditionType type, string variableName, ConditionComparison comparison, PropertyValue compareTo)
    {
        Type = type;
        VariableName = variableName;
        Comparison = comparison;
        CompareTo = compareTo;
    }

    public bool IsMet (IPositionalComponent component)
    {
        if (Type == ConditionType.Empty)
            return true;

        if (Type == ConditionType.State)
        {
            if (VariableName.ToLower () == "horizontal")
            {
                Orientation orientation = component.Layout.Orientation;

                if (Comparison == ConditionComparison.Truthy || Comparison == ConditionComparison.Equal)
                    return orientation == Orientation.Horizontal == CompareTo.BooleanValue;

                return orientation == Orientation.Horizontal != CompareTo.BooleanValue;
            }
        }
        else
        {
            if (!component.TryGetProperty (VariableName, out IComponentProperty? property))
                throw new Exception ($"Could not find property '{VariableName}' on component '{component.Name}'.");

            // TODO : Fix
            /*
            PropertyValue propertyValue = property!.Value;
            if (VariableName.StartsWith ("Show"))
                propertyValue = new PropertyValue (property.IsVisible);
            */

            PropertyValue? propertyValue = property?.Value;
            
            if (Comparison == ConditionComparison.Truthy)
                return propertyValue!.IsTruthy ();

            if (Comparison == ConditionComparison.Falsy)
                return !propertyValue!.IsTruthy ();

            int cv = propertyValue?.CompareTo (CompareTo) ?? 0;
            switch (Comparison)
            {
                case ConditionComparison.Equal:
                    return cv == 0;
                case ConditionComparison.NotEqual:
                    return cv != 0;
                case ConditionComparison.Greater:
                    return cv == 1;
                case ConditionComparison.GreaterOrEqual:
                    return cv >= 0;
                case ConditionComparison.Less:
                    return cv == -1;
                case ConditionComparison.LessOrEqual:
                    return cv <= 0;
            }
        }

        return false;
    }

    private static string ComparisonToString (ConditionComparison comparison)
    {
        switch (comparison)
        {
            case ConditionComparison.Equal:
                return "==";
            case ConditionComparison.NotEqual:
                return "!=";
            case ConditionComparison.Greater:
                return ">";
            case ConditionComparison.GreaterOrEqual:
                return ">=";
            case ConditionComparison.Less:
                return "<";
            case ConditionComparison.LessOrEqual:
                return "<=";
            default:
                return "?";
        }
    }

    public override string ToString ()
    {
        return (Type == ConditionType.Property ? "$" : "") + VariableName + ComparisonToString (Comparison) + CompareTo;
    }

    public override bool Equals (object? obj)
    {
        if (obj is not ConditionTreeLeaf o)
            return false;

        if (ReferenceEquals (this, obj))
            return true;

        bool result= Type.Equals (o.Type)
                     && Comparison.Equals (o.Comparison)
                     && VariableName.Equals (o.VariableName)
                     && CompareTo.Equals (o.CompareTo);
        
        return result;
    }

    public override int GetHashCode ()
    {
        return Type.GetHashCode ()
               ^ Comparison.GetHashCode ()
               ^ VariableName.GetHashCode ()
               ^ CompareTo.GetHashCode ();
    }
}
