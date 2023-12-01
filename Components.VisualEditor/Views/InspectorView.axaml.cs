using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Components.VisualEditor.ViewModels;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Linq;

namespace Components.VisualEditor.Views;

public partial class InspectorView : UserControl
{
    public InspectorView ()
    {
        InitializeComponent ();
    }
}
