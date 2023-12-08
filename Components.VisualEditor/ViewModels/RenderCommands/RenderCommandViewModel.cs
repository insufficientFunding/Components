using CommunityToolkit.Mvvm.ComponentModel;
using Components.Base.Enums;
using Components.IO.Xml.Parsers.Conditions;
using Components.Render.Drawing.RenderCommands;
using Components.Render.Text;
using Components.Render.TypeDescription.TypeDescription;
using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Enums;
using Components.VisualEditor.Extensions;
using Components.VisualEditor.Models;
using Components.VisualEditor.Models.Render;
using Components.VisualEditor.Parsers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RenderCommandType = Components.VisualEditor.Enums.RenderCommandType;
namespace Components.VisualEditor.ViewModels.RenderCommands;

public partial class RenderCommandViewModel : ObservableObject, IEditorRenderCommand
{
    public RenderCommandType Type { get; }

    [ObservableProperty] private string _name;

    [ObservableProperty] private ObservableCollection<IPropertyView> _properties = [];

    public RenderCommandViewModel (RenderCommandType type, string? name = null, IEnumerable<IPropertyView>? properties = null)
    {
        Type = type;
        Name = name ?? $"New {type}";

        if (properties is not null)
            Properties = new ObservableCollection<IPropertyView> (properties);
    }

    public RenderCommandViewModel ()
        : this (RenderCommandType.Rectangle, string.Empty)
    { }

    public object Flatten (ComponentDescription description, IConditionParser conditionParser)
    {
        switch (Type)
        {
            case RenderCommandType.Rectangle:
                return new Rectangle (
                    Properties.GetProperty<ComponentPointProperty> ("Position").Flatten (),
                    Properties.GetProperty<VectorProperty> ("Size").Flatten ().ToSize (),
                    Properties.GetProperty<NumericProperty> ("Thickness").Value.ParseDouble (1D),
                    Properties.GetProperty<BoolProperty> ("Fill").Value
                );

            case RenderCommandType.Ellipse:
                return new Ellipse (
                    Properties.GetProperty<ComponentPointProperty> ("Position").Flatten (),
                    Properties.GetProperty<VectorProperty> ("Radius").Flatten ().ToSize (),
                    Properties.GetProperty<NumericProperty> ("Thickness").Value.ParseDouble (1D),
                    Properties.GetProperty<BoolProperty> ("Fill").Value
                );

            case RenderCommandType.Line:
                return new Line (
                    Properties.GetProperty<ComponentPointProperty> ("Start").Flatten (),
                    Properties.GetProperty<ComponentPointProperty> ("End").Flatten (),
                    Properties.GetProperty<NumericProperty> ("Thickness").Value.ParseDouble (1D)
                );

            case RenderCommandType.Text:
                return new RenderText (
                    Properties.GetProperty<ComponentPointProperty> ("Position").Flatten (),
                    Enum.TryParse (Properties.GetProperty<EnumProperty> ("Alignment").SelectedItem?.ToString (), out TextAlignment alignment) ? alignment : TextAlignment.CenterCenter,
                    Enum.TryParse (Properties.GetProperty<EnumProperty> ("Weight").SelectedItem?.ToString (), out FontWeight weight) ? weight : FontWeight.Regular,
                    TextRotation.None,
                    [
                        new TextRun (
                            Properties.GetProperty<StringProperty> ("Text").Value,
                            new TextRunFormatting (TextRunFormattingType.Normal, (double)(Enum.TryParse (Properties.GetProperty<EnumProperty> ("Size").SelectedItem?.ToString (), out FontSize size) ? size : FontSize.Medium))
                        )
                    ]
                );
        }

        return default!;
    }
}
