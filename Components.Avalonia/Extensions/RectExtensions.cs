
using Components.Base.Primitives;
using AvaloniaRect = Avalonia.Rect;

namespace Components.Avalonia.Extensions;

/// <summary>
///     A class containing extension methods for the <see cref="Components.Base.Primitives.Rect"/> struct.
/// </summary>
public static class RectExtensions
{
    /// <summary>
    ///     Converts the given Avalonia.<see cref="AvaloniaRect"/> to a <see cref="Components.Base.Primitives.Rect"/>.
    /// </summary>
    /// <param name="rect">The Avalonia Rect to convert.</param>
    /// <returns>The converted Rect.</returns>
    public static Rect ToRect (this AvaloniaRect rect)
    {
        return new Rect (rect.X, rect.Y, rect.Width, rect.Height);
    }
    
    /// <summary>
    ///     Converts the given <see cref="Components.Base.Primitives.Rect"/> to an Avalonia.<see cref="AvaloniaRect"/>.
    /// </summary>
    /// <param name="rect">The Rect to convert.</param>
    /// <returns>The converted Avalonia rect.</returns>
    public static AvaloniaRect ToAvaloniaRect (this Rect rect)
    {
        return new AvaloniaRect (rect.X, rect.Y, rect.Width, rect.Height);
    }
}
