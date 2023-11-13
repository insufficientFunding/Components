namespace Components.Primitives;

public enum Orientation
{
    Horizontal,
    Vertical,
}
public static class OrientationExtensions
{
    public static Orientation Reverse (this Orientation orientation)
    {
        if (orientation == Orientation.Horizontal)
            return Orientation.Vertical;

        return Orientation.Horizontal;
    }

    /// <summary>
    ///     Parses the given bool value to an <see cref="Orientation"/> value.
    /// </summary>
    /// <param name="value">The bool value to parse. True indicates vertical, and false horizontal.</param>
    /// <returns>The parsed <see cref="Orientation"/> value.</returns>
    public static Orientation AsOrientation (this bool value)
    {
        return value ? Orientation.Vertical : Orientation.Horizontal;
    }
}
