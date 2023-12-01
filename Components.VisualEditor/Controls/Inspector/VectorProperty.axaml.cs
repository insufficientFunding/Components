using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Components.VisualEditor.Messages;
using Components.VisualEditor.Parsers;
using Components.VisualEditor.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Point = Components.Primitives.Point;

namespace Components.VisualEditor.Controls.Inspector;

public class VectorProperty : TemplatedControl
{
    public static readonly StyledProperty<string> PropertyNameProperty = AvaloniaProperty.Register<VectorProperty, string> (
        "PropertyName", "Vector");

    public string PropertyName
    {
        get => GetValue (PropertyNameProperty);
        set => SetValue (PropertyNameProperty, value);
    }

    #region X
    public static readonly DirectProperty<VectorProperty, string?> XProperty =
        AvaloniaProperty.RegisterDirect<VectorProperty, string?> (
            nameof (X),
            o => o.X,
            (o, v) => o.X = v,
            enableDataValidation: true);

    private string? _x;

    [CustomValidation (typeof (NumberValidation), nameof (NumberValidation.ValidateDouble))]
    public string? X
    {
        get => _x;
        set => SetAndRaise (XProperty, ref _x, value);
    }
    #endregion
    
    #region Y
    public static readonly DirectProperty<VectorProperty, string?> YProperty =
        AvaloniaProperty.RegisterDirect<VectorProperty, string?> (
            nameof (Y),
            o => o.Y,
            (o, v) => o.Y = v,
            enableDataValidation: true);

    private string? _y;

    [CustomValidation (typeof (NumberValidation), nameof (NumberValidation.ValidateDouble))]
    public string? Y
    {
        get => _y;
        set => SetAndRaise (YProperty, ref _y, value);
    }
    #endregion

    public VectorProperty (string propertyName, Point point)
    {
        PropertyName = propertyName;

        Console.WriteLine (propertyName);

        X = point.X.ToString (CultureInfo.InvariantCulture);
        Y = point.Y.ToString (CultureInfo.InvariantCulture);
    }

    public VectorProperty () { Console.WriteLine ("hi again"); }

    /// <summary>
    ///     Flattens the property into a single <see cref="Point"/>
    /// </summary>
    /// <returns>The flattened <see cref="Point"/>. If th values are not valid, they are set to 0</returns>
    public Point Flatten ()
    {
        double x = X.ParseDouble ();
        double y = Y.ParseDouble ();
        
        return new Point (x, y);
    }

    protected override void OnPropertyChanged (AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged (change);

        if (change.OldValue == change.NewValue)
            return;

        if (change.Property == XProperty
            || change.Property == YProperty)
            WeakReferenceMessenger.Default.Send (new EditorValueChangedMessage (nameof (VectorProperty)));
    }
}
