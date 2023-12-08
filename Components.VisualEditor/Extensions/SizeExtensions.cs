using Components.Base.Primitives;
namespace Components.VisualEditor.Extensions;

public static class SizeExtensions
{
    public static Size ToSize (this Point point) => new Size (point.X, point.Y);
}