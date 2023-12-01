namespace Components.Render.TypeDescription.Conditions;

public struct ConditionToken
{
    public enum TokenType : byte
    {
        Symbol = 1,
        LeftBracket = 2,
        RightBracket = 3,
        Operator = 4,
    }

    public enum OperatorType : byte
    {
        None = 0,
        AND = 1,
        OR = 2,
    }

    public TokenType Type { get; set; }
    public OperatorType Operator { get; set; }
    public string Symbol { get; set; } = null!;
    public int Position { get; set; }

    public ConditionToken (TokenType type)
        : this ()
    {
        Type = type;
        Operator = OperatorType.None;
    }

    public ConditionToken (OperatorType op)
        : this ()
    {
        Type = TokenType.Operator;
        Operator = op;
    }

    public ConditionToken (string symbol, int position)
        : this ()
    {
        Type = TokenType.Symbol;
        Operator = OperatorType.None;
        Symbol = symbol;
        Position = position;
    }

    public static ConditionToken LeftBracket = new ConditionToken (TokenType.LeftBracket);
    public static ConditionToken RightBracket = new ConditionToken (TokenType.RightBracket);
    public static ConditionToken AND = new ConditionToken (OperatorType.AND);
    public static ConditionToken OR = new ConditionToken (OperatorType.OR);

    public override bool Equals (object? obj)
    {
        if (obj == null || GetType () != obj.GetType ())
            return false;

        ConditionToken other = (ConditionToken)obj;
        return Operator == other.Operator && Type == other.Type && Symbol == other.Symbol;
    }

    public override int GetHashCode ()
    {
        return Type.GetHashCode () + Operator.GetHashCode ();
    }

    public static bool operator == (ConditionToken t1, ConditionToken t2)
    {
        return t1.Equals (t2);
    }

    public static bool operator != (ConditionToken t1, ConditionToken t2)
    {
        return !t1.Equals (t2);
    }

    public override string ToString ()
    {
        if (Type == TokenType.Symbol)
            return Symbol; 
        
        return Type.ToString ();
    }
}
