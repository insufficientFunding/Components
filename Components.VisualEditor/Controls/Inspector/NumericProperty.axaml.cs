using Avalonia;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Messaging;
using Components.VisualEditor.Messages;
using Components.VisualEditor.Models;
using Components.VisualEditor.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Components.VisualEditor.Controls.Inspector;

public class NumericProperty : TemplatedControl, IPropertyView
{
    public static readonly StyledProperty<string> PropertyNameProperty = AvaloniaProperty.Register<NumericProperty, string> (
        "PropertyName", "Name");

    public string PropertyName
    {
        get => GetValue (PropertyNameProperty);
        set => SetValue (PropertyNameProperty, value);
    }

    public static readonly DirectProperty<NumericProperty, string?> ValueProperty =
        AvaloniaProperty.RegisterDirect<NumericProperty, string?> (
            nameof (Value),
            o => o.Value,
            (o, v) => o.Value = v,
            enableDataValidation: true);

    private string? _value;

    [CustomValidation (typeof (NumberValidation), nameof (NumberValidation.ValidateDouble))]
    public string? Value
    {
        get => _value;
        set => SetAndRaise (ValueProperty, ref _value, value);
    }
    
    public NumericProperty (string propertyName, double value)
    {
        PropertyName = propertyName;
        Value = value.ToString (CultureInfo.InvariantCulture);
    }

    protected override void OnPropertyChanged (AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged (change);

        if (change.OldValue == change.NewValue)
            return;

        if (change.Property == ValueProperty)
            WeakReferenceMessenger.Default.Send (new EditorValueChangedMessage (nameof (NumericProperty)));
    }

    public NumericProperty ()
    { }
}
