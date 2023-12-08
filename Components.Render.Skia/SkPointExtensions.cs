using Components.Base.Primitives;
using SkiaSharp;
namespace Components.Render.Skia;

public static class SkPointExtensions
{
    /// <summary>
    ///    Scales a <see cref="SkiaSharp.SKPoint"/> by a given scale.
    /// </summary>
    /// <param name="point">The point to scale.</param>
    /// <param name="scale">The scale to apply.</param>
    /// <returns>The scaled point.</returns>
    public static SKPoint Scale (this SKPoint point, float scale)
    {
        return new SKPoint (
            point.X * scale,
            point.Y * scale);
    }

    /// <summary>
    ///   Scales a <see cref="SkiaSharp.SKPoint"/> to a given canvas size.
    /// </summary>
    /// <param name="point">The point to scale.</param>
    /// <param name="canvasSize">The size of the canvas to scale to.</param>
    /// <param name="scaleFrom">The size of the canvas to scale from.</param>
    /// <returns>The scaled point.</returns>
    public static SKPoint ScaleToCanvasSize (this SKPoint point, double canvasSize, float scaleFrom = 6f)
    {
        float scaleFactor = (float)canvasSize / scaleFrom;

        return new SKPoint (
            point.X * scaleFactor,
            point.Y * scaleFactor);
    }

    /// <summary>
    ///   Scales a <see cref="SkiaSharp.SKSize"/> to a given canvas size.
    /// </summary>
    /// <param name="size">The size to scale.</param>
    /// <param name="canvasSize">The size of the canvas to scale to.</param>
    /// <param name="scaleFrom">The size of the canvas to scale from.</param>
    /// <returns>The scaled point.</returns>
    public static SKSize ScaleToCanvasSize (this SKSize size, double canvasSize, float scaleFrom = 6f)
    {
        float scaleFactor = (float)canvasSize / scaleFrom;

        return new SKSize (
            size.Width * scaleFactor,
            size.Height * scaleFactor);
    }

    /// <summary>
    ///     Converts a <see cref="Components.Primitives.Point"/> to a <see cref="SkiaSharp.SKPoint"/>.
    /// </summary>
    /// <param name="point">The point to convert.</param>
    /// <returns>The converted point.</returns>
    public static SKPoint ToSkPoint (this Point point)
    {
        return new SKPoint ((float)point.X, (float)point.Y);
    }

    /// <summary>
    ///     Converts a <see cref="Components.Primitives.Size"/> to a <see cref="SkiaSharp.SKSize"/>.
    /// </summary>
    /// <param name="size">The Avalonia point to convert.</param>
    /// <returns>The converted point.</returns>
    public static SKSize ToSkSize (this Size size)
    {
        return new SKSize ((float)size.Width, (float)size.Height);
    }
}
