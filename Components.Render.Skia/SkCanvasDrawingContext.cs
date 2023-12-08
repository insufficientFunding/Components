using Components.Base.Enums;
using Components.Base.Primitives;
using Components.Render.Drawing.DrawingContext;
using Components.Render.Drawing.RenderCommands.Path;
using Components.Render.Text;
using SkiaSharp;
using System.Text;

namespace Components.Render.Skia;

/// <inheritdoc cref="IDrawingContext"/>
/// <summary>
///     A <see cref="IDrawingContext"/> implementation for <see cref="SkiaSharp.SKCanvas"/>.
/// </summary>
public class SkCanvasDrawingContext : IDrawingContext
{
    /// <summary>
    ///     The typeface family to use when rendering text.
    /// </summary>
    public SKTypefaceFamily? FontTypeface { get; private set; }
    
    /// <summary>
    ///     The <see cref="SkiaSharp.SKCanvas"/> to draw to.
    /// </summary>
    public SKCanvas? SkCanvas { get; set; }

    /// <summary>
    ///     The size of the bounds to draw to.
    /// </summary>
    public double BoundsSize { get; set; } = 120D;

    /// <summary>
    ///     The size of the component's rendering area.
    /// </summary>
    public double ComponentSize { get; set; } = 100D;

    /// <summary>
    ///     The color to draw with.
    /// </summary>
    public SKColor Color { get; set; } = SKColors.White;
    
    /// <summary>
    ///     The multiplier to apply to the stroke thickness.
    /// </summary>
    public double StrokeThicknessMultiplier { get; set; } = 1;

    public void SetTypeface (SKTypefaceFamily? typeface)
    {
        FontTypeface = typeface;
    }

    public void DrawLine (Point start, Point end, double thickness)
    {
        SKPaint paint = new SKPaint
        {
            Color = Color,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = (float)thickness * (float)StrokeThicknessMultiplier,
            StrokeCap = SKStrokeCap.Round,
        };

        SKPoint startPoint = start.ToSkPoint ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize);
        SKPoint endPoint = end.ToSkPoint ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize);

        SkCanvas?.DrawLine (startPoint, endPoint, paint);
    }

    public void DrawRectangle (Point start, Size size, double thickness, bool fill = false)
    {
        SKPaint paint = new SKPaint
        {
            Color = Color,
            IsAntialias = true,
            Style = fill ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke,
            StrokeWidth = (float)thickness * (float)StrokeThicknessMultiplier,
            StrokeCap = SKStrokeCap.Round,
        };

        SKPoint startPoint = start.ToSkPoint ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize);
        SKSize skSize = size.ToSkSize () .ScaleToCanvasSize (BoundsSize, (float)ComponentSize);

        SkCanvas?.DrawRect (startPoint.X, startPoint.Y, skSize.Width, skSize.Height, paint);
    }

    public void DrawEllipse (Point center, Size radius, double thickness, bool fill = false)
    {
        SKPaint paint = new SKPaint
        {
            Color = Color,
            IsAntialias = true,
            Style = fill ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke,
            StrokeWidth = (float)thickness * (float)StrokeThicknessMultiplier,
            StrokeCap = SKStrokeCap.Round,
        };

        SKPoint centerPoint = center.ToSkPoint ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize);
        SKSize skiaRadius = radius.ToSkSize  ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize);

        SkCanvas?.DrawOval (centerPoint.X, centerPoint.Y, skiaRadius.Width, skiaRadius.Height, paint);
    }

    public void DrawPath (Point start, List<IPathCommand> commands, double thickness, bool fill = false)
    {
        SKPaint paint = new SKPaint
        {
            Color = Color,
            IsAntialias = true,
            Style = fill ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke,
            StrokeWidth = (float)thickness * (float)StrokeThicknessMultiplier,
            StrokeCap = SKStrokeCap.Round,
        };

        SKPoint startPoint = start.ToSkPoint ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize);

        SKPath path = new SKPath ();
        path.MoveTo (startPoint);
        foreach (IPathCommand command in commands)
        {
            SKPoint endPoint = command.End.ToSkPoint ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize);
            endPoint = command.Relative ? path.LastPoint + endPoint : endPoint;

            switch (command)
            {
                case MoveTo:
                    path.MoveTo (endPoint);

                    break;
                case LineTo:
                    path.LineTo (endPoint);

                    break;
                case EllipticalArcTo arcTo:
                    SKPathArcSize pathArcSize = arcTo.IsLargeArc ? SKPathArcSize.Large : SKPathArcSize.Small;
                    SKPathDirection pathDirection = arcTo.SweepDirection == SweepDirection.Clockwise ? SKPathDirection.Clockwise : SKPathDirection.CounterClockwise;

                    path.ArcTo (
                        arcTo.Radii.ToSkPoint ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize),
                        (float)arcTo.Angle,
                        pathArcSize,
                        pathDirection,
                        endPoint);

                    break;
                case ClosePath _:
                    path.Close ();

                    break;
            }
        }

        SkCanvas?.DrawPath (path, paint);
    }

    public void DrawText (Point anchor, TextAlignment alignment, FontWeight weight, double rotation, IList<TextRun> textRuns)
    {
        SKPaint paint = new SKPaint
        {
            Color = Color,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            TextSize = 12f,
            Typeface = FontTypeface.GetSKTypeface (weight),
            SubpixelText = true,
            IsLinearText = true,
        };

        float totalWidth = 0f;
        float totalHeight = 0f;

        foreach (TextRun run in textRuns)
        {
            if (string.IsNullOrEmpty (run.Text))
                continue;

            paint.TextSize = (float)run.Formatting.Size;

            if (run.Formatting.FormattingType == TextRunFormattingType.Subscript ||
                run.Formatting.FormattingType == TextRunFormattingType.Superscript)
                paint.TextSize /= 1.5f;

            SKRect bounds = new SKRect ();
            paint.MeasureText (Encoding.UTF8.GetBytes (run.Text), ref bounds);
            totalWidth += bounds.Right;
            totalHeight = Math.Max (totalHeight, bounds.Height);
        }

        SKPoint startLocation = anchor.ToSkPoint ().ScaleToCanvasSize (BoundsSize, (float)ComponentSize);

        SkCanvas?.Save ();
        SkCanvas?.RotateDegrees ((float)rotation, startLocation.X, startLocation.Y);

        // Horizontal alignment
        if (alignment == TextAlignment.TopCenter || alignment == TextAlignment.CenterCenter || alignment == TextAlignment.BottomCenter)
            startLocation.X -= totalWidth / 2;
        else if (alignment == TextAlignment.TopRight || alignment == TextAlignment.CenterRight || alignment == TextAlignment.BottomRight)
            startLocation.X -= totalWidth;

        // Vertical alignment
        if (alignment == TextAlignment.TopLeft || alignment == TextAlignment.TopCenter || alignment == TextAlignment.TopRight)
            startLocation.Y += totalHeight;
        if (alignment == TextAlignment.CenterLeft || alignment == TextAlignment.CenterCenter || alignment == TextAlignment.CenterRight)
            startLocation.Y += totalHeight / 2;

        float horizontalOffsetCounter = 0;
        foreach (TextRun run in textRuns)
        {
            if (string.IsNullOrEmpty (run.Text))
                continue;

            paint.TextSize = (float)run.Formatting.Size;
            SKPoint renderLocation = new SKPoint (startLocation.X + horizontalOffsetCounter, startLocation.Y);

            if (run.Formatting.FormattingType == TextRunFormattingType.Subscript)
            {
                paint.TextSize /= 1.5f;
                renderLocation.Y += 3f;
            }
            else if (run.Formatting.FormattingType == TextRunFormattingType.Superscript)
            {
                paint.TextSize /= 1.5f;
                renderLocation.Y -= 3f;
            }

            SKRect bounds = new SKRect ();
            paint.MeasureText (run.Text, ref bounds);

            SkCanvas?.DrawText (run.Text, renderLocation.X, renderLocation.Y, paint);
            horizontalOffsetCounter += bounds.Right;
        }

        SkCanvas?.Restore ();
    }

    public void Dispose ()
    {
        SkCanvas?.Dispose ();
    }
}
