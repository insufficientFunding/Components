namespace Components.Base.Internal;

/// <summary>
/// Encapsulates a non-null value.
/// </summary>
/// <typeparam name="T">Type of value to store.</typeparam>
public abstract class ObjValue<T>
{
    protected ObjValue (T value)
    {
        if (value == null)
            throw new ArgumentException ("Value cannot be null. ", nameof (value));

        Value = value;
    }

    protected T Value { get; }

    public override string? ToString ()
    {
        return Value?.ToString ();
    }
}
