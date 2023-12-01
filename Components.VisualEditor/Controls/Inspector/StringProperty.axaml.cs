using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Messaging;
using Components.VisualEditor.Messages;

namespace Components.VisualEditor.Controls.Inspector;

public class StringProperty : TemplatedControl
{
    public static readonly StyledProperty<string> PropertyNameProperty = AvaloniaProperty.Register<StringProperty, string> (
        "PropertyName", "Name");

    public string PropertyName
    {
        get => GetValue (PropertyNameProperty);
        set => SetValue (PropertyNameProperty, value);
    }

    public static readonly StyledProperty<string> ValueProperty = AvaloniaProperty.Register<StringProperty, string> (
        "Value", string.Empty);

    public string Value
    {
        get => GetValue (ValueProperty);
        set => SetValue (ValueProperty, value);
    }

    public StringProperty (string name, string defaultValue = "")
    {
        PropertyName = name;
        Value = defaultValue;
    }
    
    public StringProperty ()
    {}
    
    protected override void OnPropertyChanged (AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged (change);

        if (change.OldValue == change.NewValue)
            return;

        if (change.Property == ValueProperty)
            WeakReferenceMessenger.Default.Send (new EditorValueChangedMessage (nameof (StringProperty)));
    }
}
