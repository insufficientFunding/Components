using Components.Base.Primitives;
using AvaloniaSize = Avalonia.Size;

namespace Components.Avalonia.Extensions;

/// <summary>
///     A class containing extension methods for the <see cref="Components.Base.Primitives.Size"/> struct.
/// </summary>
public static class SizeExtensions
{
    /// <summary>
    ///     Converts the given Avalonia.<see cref="AvaloniaSize"/> to a <see cref="Components.Base.Primitives.Size"/>.
    /// </summary>
    /// <param name="size">The Size to convert.</param>
    /// <returns>The converted Size.</returns>
    public static Size ToSize (this AvaloniaSize size)
    {
        return new Size (size.Width, size.Height);
    }

    /// <summary>
    ///     Converts the given <see cref="Components.Base.Primitives.Size"/> to an Avalonia.<see cref="AvaloniaSize"/>.
    /// </summary>
    /// <param name="size">The Size to convert.</param>
    /// <returns>The converted Avalonia size.</returns>
    public static AvaloniaSize ToAvaloniaSize (this Size size)
    {
        return new AvaloniaSize (size.Width, size.Height);
    }
}
