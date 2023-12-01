using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Components.Enums;
using Components.Extensions;
using Components.Interfaces.TypeDescription;
using Components.Render.Drawing.RenderCommands.Path;
using Components.Render.TypeDescription;
using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Enums;
using Components.VisualEditor.Models;
using Components.VisualEditor.Parsers;
using Components.Xml.Parsers.Conditions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using static Components.VisualEditor.Extensions.RenderCommandExtensions;

namespace Components.VisualEditor.ViewModels.RenderCommands;

/// <summary>
///     A ViewModel for a <see cref="Enums.RenderCommandType.Path" />.
/// </summary>
public partial class PathCommandViewModel : ObservableObject
{
    public PathCommandType Type { get; }

    [ObservableProperty] private ObservableCollection<object> _properties = [];
    
    public PathCommandViewModel (PathCommandType type, IEnumerable<object>? properties = null)
    {
        Type = type;
        
        if (properties is not null)
            Properties = new ObservableCollection<object> (properties);
    }

    public object Flatten (IComponentDescription description, IConditionParser conditionParser)
    {
        switch (Type)
        {
            case PathCommandType.MoveTo:
                return new MoveTo (
                    Properties.GetProperty<VectorProperty> ("Position").Flatten (),
                    Properties.GetProperty<BoolProperty> ("Relative").Value
                );

            case PathCommandType.LineTo:
                return new LineTo (
                    Properties.GetProperty<VectorProperty> ("Position").Flatten (),
                    Properties.GetProperty<BoolProperty> ("Relative").Value
                );

            case PathCommandType.EllipticalArcTo:
                return new EllipticalArcTo (
                    Properties.GetProperty<VectorProperty> ("Radii").Flatten (),
                    Properties.GetProperty<VectorProperty> ("Position").Flatten (),
                    Properties.GetProperty<NumericProperty> ("Angle").Value.ParseDouble (1D),
                    Properties.GetProperty<BoolProperty> ("Is Large Arc").Value,
                    (SweepDirection)Enum.Parse (typeof (SweepDirection), (Properties.GetProperty<EnumProperty> ("Sweep Direction").SelectedItem as string)!),
                    Properties.GetProperty<BoolProperty> ("Relative").Value
                );

            case PathCommandType.ClosePath:
                return new ClosePath ();

            default:
                return null!;
        }
    }
}