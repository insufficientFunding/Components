using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using System;
namespace Components.VisualEditor.Controls;

public class GridDecorator : Decorator
{
    public static readonly StyledProperty<bool> EnableGridProperty =
        AvaloniaProperty.Register<GridDecorator, bool> (nameof (EnableGrid));

    public static readonly StyledProperty<double> GridCellSizeProperty =
        AvaloniaProperty.Register<GridDecorator, double> (nameof (GridCellSize));

    public static readonly StyledProperty<IBrush> GridBrushProperty =
        AvaloniaProperty.Register<GridDecorator, IBrush> (nameof (GridBrush));

    public bool EnableGrid
    {
        get => GetValue (EnableGridProperty);
        set => SetValue (EnableGridProperty, value);
    }

    public double GridCellSize
    {
        get => GetValue (GridCellSizeProperty);
        set => SetValue (GridCellSizeProperty, value);
    }

    public IBrush GridBrush
    {
        get => GetValue (GridBrushProperty);
        set => SetValue (GridBrushProperty, value);
    }

    protected override void OnPropertyChanged (AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged (change);

        if (change.Property == EnableGridProperty
            || change.Property == GridCellSizeProperty)
            InvalidateVisual ();
    }

    public override void Render (DrawingContext context)
    {
        base.Render (context);

        if (!EnableGrid)
            return;

        int cellSize = (int)Math.Round (GridCellSize);

        if (cellSize <= 0 || cellSize <= 0.0)
            return;

        Rect rect = Bounds;
        double thickness = 0.5;

        IImmutableBrush? brush = GridBrush.ToImmutable ();
        ImmutablePen? pen = new ImmutablePen (brush, thickness);

        using DrawingContext.PushedState _ = context.PushTransform (Matrix.CreateTranslation (-0.5d, -0.5d));

        double ox = rect.X;
        double ex = rect.X + rect.Width;
        double oy = rect.Y;
        double ey = rect.Y + rect.Height;
        for (double x = ox + cellSize; x < ex; x += cellSize)
        {
            Point p0 = new Point (x + 0.5, oy + 0.5);
            Point p1 = new Point (x + 0.5, ey + 0.5);
            context.DrawLine (pen, p0, p1);
        }

        for (double y = oy + cellSize; y < ey; y += cellSize)
        {
            Point p0 = new Point (ox + 0.5, y + 0.5);
            Point p1 = new Point (ex + 0.5, y + 0.5);
            context.DrawLine (pen, p0, p1);
        }
        context.DrawRectangle (null, pen, rect);
    }
}
