using Components.Base.Internal;
namespace Components.Base.DataModels;

/// <summary>
///     Represents the name of a property.
/// </summary>
public class PropertyName : ObjValue<string>
{
    public PropertyName (string value) : base (value)
    { }
    
    #region Equality and Operators
    protected bool Equals (PropertyName other)
    {
        return Value.Equals (other.Value);
    }

    public override bool Equals (object? obj)
    {
        if (ReferenceEquals (null, obj)) return false;
        if (ReferenceEquals (this, obj)) return true;
        if (obj.GetType () != GetType ()) return false;
        return Equals ((PropertyName)obj);
    }

    public static bool operator == (PropertyName left, PropertyName right)
    {
        return Equals (left, right);
    }

    public static bool operator != (PropertyName left, PropertyName right)
    {
        return !Equals (left, right);
    }

    public static implicit operator PropertyName (string value)
    {
        return new PropertyName (value);
    }
    
    public static implicit operator string (PropertyName value)
    {
        return value.Value;
    }
    #endregion
    
    public override int GetHashCode ()
    {
        return Value.GetHashCode ();
    }
}
