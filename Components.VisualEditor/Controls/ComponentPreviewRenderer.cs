using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Components.Avalonia;
using Components.Base.Models;
using Components.Render.Drawing;
using Components.Render.Skia;
using Components.Render.TypeDescription;
using Components.VisualEditor.Messages;
using Components.VisualEditor.ViewModels;
using SkiaSharp;
using System.Collections.Generic;
namespace Components.VisualEditor.Controls;

public class ComponentPreviewRenderer : SkiaCanvas, IRecipient<RenderPreviewMessage>
{
    private IPositionalComponent? _previewComponent;
    private ILayoutContext? _layoutContext;
    private readonly SkCanvasDrawingContext _drawingContext;
    private IEnumerable<RenderDescription>? _flattenedRenderDescriptions;

    public ComponentPreviewRenderer (double width, double height) : base (width, height)
    {
        _drawingContext = new SkCanvasDrawingContext
        {
            BoundsSize = 100,
            ComponentSize = 100,
        };

        WeakReferenceMessenger.Default.Register (this);
    }

    public ComponentPreviewRenderer ()
        : this (100, 100)
    { }

    protected override void OnInitialized ()
    {
        base.OnInitialized ();

        RenderSkia += HandleRenderSkia;
    }

    public void Receive (RenderPreviewMessage message)
    {
        _previewComponent ??= new PositionalComponent ("Preview Component");
        _layoutContext ??= new LayoutContext ();

        _previewComponent.Layout.Size = 100D;

        Dispatcher.UIThread.Post (() => _flattenedRenderDescriptions = (DataContext as EditorViewModel)?.PreviewDescription.RenderDescriptions, DispatcherPriority.MaxValue);

        InvalidateVisual ();
    }

    private void HandleRenderSkia (SKCanvas canvas)
    {
        if (!IsInitialized
            || _previewComponent is null
            || _layoutContext is null
            || _flattenedRenderDescriptions is null)
            return;

        _drawingContext.SkCanvas = canvas;

        foreach (var renderGroup in _flattenedRenderDescriptions)
        {
            if (renderGroup.Conditions.IsMet (_previewComponent))
                renderGroup.Render (_drawingContext, _layoutContext, _previewComponent.Layout);
        }
    }
}
