using Avalonia;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Messaging;
using Components.VisualEditor.Messages;
using Components.VisualEditor.Models;

namespace Components.VisualEditor.Controls.Inspector;

public class BoolProperty : TemplatedControl, IPropertyView
{
    public static readonly StyledProperty<string> PropertyNameProperty = AvaloniaProperty.Register<BoolProperty, string> (
        "PropertyName", "Bool");

    public string PropertyName
    {
        get => GetValue (PropertyNameProperty);
        set => SetValue (PropertyNameProperty, value);
    }

    public static readonly StyledProperty<bool> ValueProperty = AvaloniaProperty.Register<BoolProperty, bool> (
        "Value");

    public bool Value
    {
        get => GetValue (ValueProperty);
        set => SetValue (ValueProperty, value);
    }

    public BoolProperty (string propertyName, bool value)
    {
        PropertyName = propertyName;
        Value = value;
    }

    public BoolProperty () { }

    protected override void OnPropertyChanged (AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged (change);

        if (change.OldValue == change.NewValue)
            return;

        if (change.Property != ValueProperty)
            return;
        
        WeakReferenceMessenger.Default.Send (new EditorValueChangedMessage (PropertyName));
    }

}
