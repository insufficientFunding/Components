using Components.Base.Extensions;
using System.Globalization;
namespace Components.Base.DataModels;

/// <summary>
///     Represents a property value of different types.
/// </summary>
public sealed class PropertyValue : IComparable<PropertyValue>, IEquatable<PropertyValue>
{
    public PropertyValue ()
    {
        PropertyType = Type.Unset;
    }

    private PropertyValue (Type type)
    {
        PropertyType = type;
    }

    private PropertyValue (string stringValue, Type type)
        : this (type)
    {
        StringValue = stringValue;
    }

    public PropertyValue (string value)
        : this (Type.String)
    {
        StringValue = value;
    }

    public PropertyValue (double value)
        : this (Type.Numeric)
    {
        NumericValue = value;
    }

    public PropertyValue (bool value)
        : this (Type.Boolean)
    {
        BooleanValue = value;
    }

    public static PropertyValue Dynamic (string value)
    {
        return new PropertyValue (value, Type.Unknown);
    }

    public string? StringValue { get; }
    public double NumericValue { get; private set; }
    public bool BooleanValue { get; }

    public void Match (Action<string> stringAction, Action<double> numericAction, Action<bool> boolAction)
    {
        switch (PropertyType)
        {
            case Type.Boolean:
                boolAction (BooleanValue);
                return;
            case Type.Numeric:
                numericAction (NumericValue);
                return;
            case Type.String:
                stringAction (StringValue!);
                return;
            case Type.Unknown:
                if (StringValue != null)
                    stringAction (StringValue);
                return;
        }
    }

    public Type PropertyType { get; private set; }

    public bool IsNumeric ()
    {
        if (PropertyType == Type.Numeric)
            return true;

        if (double.TryParse (StringValue, out double val))
        {
            PropertyType = Type.Numeric;
            NumericValue = val;
            return true;
        }

        return false;
    }

    #region Equality and comparisons
    public bool Equals (PropertyValue? other)
    {
        if (other is null)
            return false;

        PropertyValue thisProperty = this;
        if (PropertyType == Type.Unknown && other.PropertyType != Type.Unknown)
        {
            switch (other.PropertyType)
            {
                case Type.String:
                    thisProperty = new PropertyValue (ToString ());
                    break;
                case Type.Numeric:
                    thisProperty = Parse (ToString (), Type.Numeric);
                    break;
                case Type.Boolean:
                    thisProperty = new PropertyValue (ToString ().ToLowerInvariant () == "true");
                    break;
            }
        }

        if (thisProperty.PropertyType != other.PropertyType)
            return false;

        switch (thisProperty.PropertyType)
        {
            case Type.Boolean:
                return thisProperty.BooleanValue.Equals (other.BooleanValue);
            case Type.Numeric:
                return thisProperty.NumericValue.Equals (other.NumericValue);
            case Type.String:
                return thisProperty.StringValue!.Equals (other.StringValue);
            case Type.Unset:
                return Equals (thisProperty.BooleanValue, other.BooleanValue)
                       && Equals (thisProperty.NumericValue, other.NumericValue)
                       && Equals (thisProperty.StringValue, other.StringValue);
            default:
                return false;
        }
    }

    public int CompareTo (PropertyValue? other)
    {
        if (other == null)
            return 0;

        PropertyValue? thisProperty = this;
        if (PropertyType == Type.Unknown && other.PropertyType != Type.Unknown)
        {
            switch (other.PropertyType)
            {
                case Type.String:
                    thisProperty = new PropertyValue (ToString ());
                    break;
                case Type.Numeric:
                    thisProperty = Parse (ToString (), Type.Numeric);
                    break;
                case Type.Boolean:
                    thisProperty = new PropertyValue (ToString ().ToLowerInvariant () == "true");
                    break;
            }
        }

        if (thisProperty.PropertyType != other.PropertyType)
            throw new InvalidOperationException ("Cannot compare property values of different types.");

        switch (thisProperty.PropertyType)
        {
            case Type.Boolean:
                return thisProperty.BooleanValue.CompareTo (other.BooleanValue);
            case Type.Numeric:
                return thisProperty.NumericValue.CompareTo (other.NumericValue);
            case Type.String:
                return string.Compare (thisProperty.StringValue, other.StringValue, StringComparison.Ordinal);
            default:
                return 0;
        }
    }
    #endregion

    public override string ToString ()
    {
        string result = string.Empty;

        Match (
            stringAction => result = stringAction,
            numericAction => result = numericAction.ToString (CultureInfo.InvariantCulture),
            boolAction => result = boolAction.ToString (CultureInfo.InvariantCulture)
        );

        return result;
    }

    public static PropertyValue Parse (string? value, Type parseAs, bool unsetIfNull = false)
    {
        if (string.IsNullOrEmpty (value))
            return unsetIfNull ? new PropertyValue () : new PropertyValue (parseAs);

        if (bool.TryParse (value, out bool b))
            return new PropertyValue (b);

        switch (parseAs)
        {
            case Type.Boolean:
                return new PropertyValue (bool.Parse (value));
            case Type.Numeric:
                return new PropertyValue (value.ParseDouble ());
            case Type.String:
                return new PropertyValue (value);
            default:
                return new PropertyValue ();
        }
    }

    public enum Type
    {
        String,
        Numeric,
        Boolean,
        Unknown,
        Unset,
    }
}
