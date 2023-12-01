using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Messaging;
using Components.VisualEditor.Messages;
using System.Collections;
using System.Collections.Generic;

namespace Components.VisualEditor.Controls.Inspector;

public class EnumProperty : TemplatedControl
{
    public static readonly StyledProperty<string> PropertyNameProperty = AvaloniaProperty.Register<EnumProperty, string> (
        "PropertyName");

    public string PropertyName
    {
        get => GetValue (PropertyNameProperty);
        set => SetValue (PropertyNameProperty, value);
    }

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty = AvaloniaProperty.Register<EnumProperty, IEnumerable?> (
        "ItemsSource");

    public IEnumerable? ItemsSource
    {
        get => GetValue (ItemsSourceProperty);
        set => SetValue (ItemsSourceProperty, value);
    }

    public static readonly StyledProperty<object?> SelectedItemProperty = AvaloniaProperty.Register<EnumProperty, object?> (
        "SelectedItem");

    public object? SelectedItem
    {
        get => GetValue (SelectedItemProperty);
        set => SetValue (SelectedItemProperty, value);
    }

    public EnumProperty ()
    { }

    public EnumProperty (string propertyName, IEnumerable? itemsSource, string? defaultValue = null)
    {
        PropertyName = propertyName;
        ItemsSource = itemsSource;
        SelectedItem = defaultValue;
    }

    protected override void OnPropertyChanged (AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged (change);

        if (change.OldValue == change.NewValue)
            return;

        if (change.Property == SelectedItemProperty)
            WeakReferenceMessenger.Default.Send (new EditorValueChangedMessage (nameof (EnumProperty)));
    }
}
