using Avalonia;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Messaging;
using Components.Render.TypeDescription;
using Components.VisualEditor.Messages;
using Components.VisualEditor.Models;
using Components.VisualEditor.Parsers;
using Components.VisualEditor.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Components.VisualEditor.Controls.Inspector;

public class ComponentPointProperty : TemplatedControl, IPropertyView
{
    public static readonly StyledProperty<string> PropertyNameProperty = AvaloniaProperty.Register<ComponentPointProperty, string> (
        "PropertyName");

    public string PropertyName
    {
        get => GetValue (PropertyNameProperty);
        set => SetValue (PropertyNameProperty, value);
    }

    public static readonly StyledProperty<string> AnchorXProperty = AvaloniaProperty.Register<ComponentPointProperty, string> (
        "AnchorX", "Middle");

    public string AnchorX
    {
        get => GetValue (AnchorXProperty);
        set => SetValue (AnchorXProperty, value);
    }

    public static readonly StyledProperty<string> AnchorYProperty = AvaloniaProperty.Register<ComponentPointProperty, string> (
        "AnchorY", "Middle");

    public string AnchorY
    {
        get => GetValue (AnchorYProperty);
        set => SetValue (AnchorYProperty, value);
    }

    #region OffsetX
    public static readonly DirectProperty<ComponentPointProperty, string?> OffsetXProperty =
        AvaloniaProperty.RegisterDirect<ComponentPointProperty, string?> (
            nameof (OffsetX),
            o => o.OffsetX,
            (o, v) => o.OffsetX = v,
            enableDataValidation: true);

    private string? _offsetX;

    [CustomValidation (typeof (NumberValidation), nameof (NumberValidation.ValidateDouble))]
    public string? OffsetX
    {
        get => _offsetX;
        set => SetAndRaise (OffsetXProperty, ref _offsetX, value);
    }
    #endregion

    #region OffsetY
    public static readonly DirectProperty<ComponentPointProperty, string?> OffsetYProperty =
        AvaloniaProperty.RegisterDirect<ComponentPointProperty, string?> (
            nameof (OffsetY),
            o => o.OffsetY,
            (o, v) => o.OffsetY = v,
            enableDataValidation: true);

    private string? _offsetY;

    [CustomValidation (typeof (NumberValidation), nameof (NumberValidation.ValidateDouble))]
    public string? OffsetY
    {
        get => _offsetY;
        set => SetAndRaise (OffsetYProperty, ref _offsetY, value);
    }
    #endregion
    
    public static List<string> Options => Enum.GetNames (typeof (ComponentPoint.Anchor)).ToList ();

    public ComponentPointProperty (string propertyName, ComponentPoint? value = null)
    {
        PropertyName = propertyName;

        if (value is null)
            return;
        
        AnchorX = value.RelativeToX.ToString ();
        AnchorY = value.RelativeToY.ToString ();

        OffsetX = value.Offset.X.ToString (CultureInfo.InvariantCulture);
        OffsetY = value.Offset.Y.ToString (CultureInfo.InvariantCulture);
    }

    public ComponentPointProperty ()
    { }

    /// <summary>
    ///     Flattens the property into a single <see cref="ComponentPoint"/>
    /// </summary>
    /// <returns>The flattened <see cref="ComponentPoint"/>. If the offset values are not valid, they are set to 0</returns>
    public ComponentPoint Flatten ()
    {
        ComponentPoint.Anchor x = (ComponentPoint.Anchor)Enum.Parse (typeof (ComponentPoint.Anchor), AnchorX);
        ComponentPoint.Anchor y = (ComponentPoint.Anchor)Enum.Parse (typeof (ComponentPoint.Anchor), AnchorY);

        double xOffset = OffsetX.ParseDouble ();
        double yOffset = OffsetY.ParseDouble ();

        return new ComponentPoint (x, y, new (xOffset, yOffset));
    }

    protected override void OnPropertyChanged (AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged (change);

        if (change.OldValue == change.NewValue)
            return;

        if (change.Property == AnchorXProperty
            || change.Property == AnchorYProperty
            || change.Property == OffsetXProperty
            || change.Property == OffsetYProperty)
            WeakReferenceMessenger.Default.Send (new EditorValueChangedMessage (nameof (ComponentPointProperty)));
    }
}
