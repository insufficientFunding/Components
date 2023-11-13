
using Components.Primitives;
using AvaloniaPoint = Avalonia.Point;

namespace Components.Avalonia.Extensions;

/// <summary>
///     A class containing extension methods for the <see cref="Components.Primitives.Point"/> struct.
/// </summary>
public static class PointExtensions
{
    /// <summary>
    ///     Converts the given Avalonia.<see cref="AvaloniaPoint"/> to a <see cref="Components.Primitives.Point"/>.
    /// </summary>
    /// <param name="point">The Point to convert.</param>
    /// <returns>The converted Point.</returns>
    public static Point ToPoint (this AvaloniaPoint point)
    {
        return new Point (point.X, point.Y);
    }

    /// <summary>
    ///     Converts the given <see cref="Components.Primitives.Point"/> to an Avalonia.<see cref="AvaloniaPoint"/>.
    /// </summary>
    /// <param name="point">The Point to convert.</param>
    /// <returns>The converted Avalonia point.</returns>
    public static AvaloniaPoint ToAvaloniaPoint (this Point point)
    {
        return new AvaloniaPoint (point.X, point.Y);
    }
}
