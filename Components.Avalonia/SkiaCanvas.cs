using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
namespace Components.Avalonia;

/// <summary>
///     A <see cref="Canvas"/> that allows you to render SkiaSharp content.
/// </summary>
public class SkiaCanvas : Control
{
    #region Rendering Logic
    /// <summary>
    ///     The rendering logic for the <see cref="SkiaCanvas"/>.
    /// </summary>
    private class RenderingLogic : ICustomDrawOperation
    {
        public Action<SKCanvas>? RenderCall;
        public Func<Point, bool>? DoHitTest;
        public Rect Bounds { get; set; }

        public void Dispose () { }

        public bool Equals (ICustomDrawOperation? other)
        {
            return other == this;
        }

        public bool HitTest (Point p)
        {
            return DoHitTest?.Invoke (p) ?? false;
        }

        public void Render (ImmediateDrawingContext context)
        {
            ISkiaSharpApiLeaseFeature? skia = context.TryGetFeature<ISkiaSharpApiLeaseFeature> ();

            if (skia is null)
                return;

            using ISkiaSharpApiLease lease = skia.Lease ();

            SKCanvas canvas = lease.SkCanvas;

            RenderCall?.Invoke (canvas);
        }

    }
    #endregion

    private readonly RenderingLogic _renderingLogic;

    public event Action<SKCanvas>? RenderSkia;
    public Func<Point, bool>? DoHitTest;


    protected SkiaCanvas (double width, double height)
    {
        Width = width;
        Height = height;

        Bounds = new Rect (0, 0, width, height);

        HorizontalAlignment = HorizontalAlignment.Left;
        VerticalAlignment = VerticalAlignment.Top;

        _renderingLogic = new RenderingLogic ();
        _renderingLogic.RenderCall += (canvas) => RenderSkia?.Invoke (canvas);
        _renderingLogic.DoHitTest += (p) => DoHitTest?.Invoke (p) ?? false;
    }

    public override void Render (DrawingContext context)
    {
        _renderingLogic.Bounds = new Rect (0, 0, Bounds.Width, Bounds.Height);

        context.Custom (_renderingLogic);
    }
}
