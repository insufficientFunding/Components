using Components.Base.Enums;
using Components.Base.Primitives;
using Components.Render.TypeDescription;
using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Enums;
using Components.VisualEditor.Models;
using Components.VisualEditor.Models.Render;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using RenderCommandType = Components.VisualEditor.Enums.RenderCommandType;
namespace Components.VisualEditor.Extensions;

public static class RenderCommandExtensions
{
    #region Enum Properties
    public static IEnumerable<string> CommandNames => Enum.GetNames (typeof (RenderCommandType));
    public static IEnumerable<string> TextAlignmentNames => Enum.GetNames (typeof (TextAlignment));
    public static IEnumerable<string> FontSizeNames => Enum.GetNames (typeof (FontSize));
    public static IEnumerable<string> FontWeightNames => Enum.GetNames (typeof (FontWeight));
    public static IEnumerable<string> PathCommandNames => Enum.GetNames (typeof (PathCommandType));
    #endregion

    #region RenderCommand Factories
    /// <summary>
    ///     Gets the <see cref="RenderCommandViewModel"/> for the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">A <see cref="RenderCommandType"/> specifying the type of the <see cref="RenderCommandViewModel"/>.</param>
    /// <param name="name">The optional name of the command.</param>
    /// <returns></returns>
    public static IEditorRenderCommand GetRenderCommand (RenderCommandType type, string? name = null)
    {
        name ??= $"New {type.ToString ()}";

        switch (type)
        {
            case RenderCommandType.Rectangle:
                return new RenderCommandViewModel (type, name, [
                    new ComponentPointProperty ("Position", new ComponentPoint (ComponentPoint.Anchor.Middle, ComponentPoint.Anchor.Middle, new Point (-10, -10))),
                    new VectorProperty ("Size", new Point (20, 20)),
                    new NumericProperty ("Thickness", 1D),
                    new BoolProperty ("Fill", false)
                ]);

            case RenderCommandType.Ellipse:
                return new RenderCommandViewModel (type, name, [
                    new ComponentPointProperty ("Position", new ComponentPoint (ComponentPoint.Anchor.Middle, ComponentPoint.Anchor.Middle, new Point ())),
                    new VectorProperty ("Radius", new Point (10, 10)),
                    new NumericProperty ("Thickness", 1D),
                    new BoolProperty ("Fill", false)
                ]);

            case RenderCommandType.Line:
                return new RenderCommandViewModel (type, name, [
                    new ComponentPointProperty ("Start", new ComponentPoint (ComponentPoint.Anchor.Middle, ComponentPoint.Anchor.Middle, new Point (-10, 0))),
                    new ComponentPointProperty ("End", new ComponentPoint (ComponentPoint.Anchor.Middle, ComponentPoint.Anchor.Middle, new Point (10, 0))),
                    new NumericProperty ("Thickness", 1D),
                ]);

            case RenderCommandType.Text:
                return new RenderCommandViewModel (type, name, [
                    new StringProperty ("Text", "Hello, World!"),
                    new ComponentPointProperty ("Position", new ComponentPoint (ComponentPoint.Anchor.Middle, ComponentPoint.Anchor.Middle, new Point ())),
                    new EnumProperty ("Alignment", TextAlignmentNames, "CenterCenter"),
                    new EnumProperty ("Size", FontSizeNames, "Medium"),
                    new EnumProperty ("Weight", FontWeightNames, "Regular"),
                ]);

            case RenderCommandType.Path:
                return new RenderPathViewModel (name, [
                    new ComponentPointProperty ("Start", new ComponentPoint (ComponentPoint.Anchor.Middle, ComponentPoint.Anchor.Middle, new Point (0, 0))),
                    new NumericProperty ("Thickness", 1D),
                    new BoolProperty ("Fill", false),
                ]);

            default:
                return new RenderGroupViewModel (name);
        }
    }

    /// <summary>
    ///     Gets a <see cref="PathCommandViewModel"/> for the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The <see cref="PathCommandType"/> of the <see cref="PathCommandViewModel"/>.</param>
    /// <returns></returns>
    public static PathCommandViewModel GetPathCommand (PathCommandType type)
    {
        switch (type)
        {
            case PathCommandType.MoveTo:
                return new PathCommandViewModel (type, [
                    new VectorProperty ("Position", new Point ()),
                    new BoolProperty ("Relative", true)
                ]);

            case PathCommandType.LineTo:
                return new PathCommandViewModel (type, [
                    new VectorProperty ("Position", new Point ()),
                    new BoolProperty ("Relative", true)
                ]);

            case PathCommandType.EllipticalArcTo:
                return new PathCommandViewModel (type, [
                    new VectorProperty ("Position", new Point ()),
                    new VectorProperty ("Radii", new Point ()),
                    new NumericProperty ("Angle", 0D),
                    new EnumProperty ("Sweep Direction", Enum.GetNames (typeof (SweepDirection)), "Clockwise"),
                    new BoolProperty ("Is Large Arc", false),
                    new BoolProperty ("Relative", true),
                ]);

            case PathCommandType.ClosePath:
                return new PathCommandViewModel (type);

            default:
                return null!;
        }
    }
    #endregion

    #region Property Search
    /// <summary>
    ///     Gets a property from a collection of type <typeparamref name="TCollection"/>, where the returned property is of type <typeparamref name="TTarget"/>.
    /// </summary>
    /// <param name="collection">The collection of type <typeparamref name="TCollection"/> to get the property from.</param>
    /// <param name="propertyName">The name of the property to append</param>
    /// <typeparam name="TTarget">The type of the property being appended.</typeparam>
    /// <typeparam name="TCollection">The type of the collection to get the property from.</typeparam>
    /// <returns>The property of type <typeparamref name="TTarget"/>.</returns>
    public static TTarget GetProperty<TTarget, TCollection> (this IEnumerable<TCollection> collection, string propertyName)
    {
        var property = typeof (TTarget).GetProperty ("PropertyName");

        return collection.Where (x => x?.GetType () == typeof (TTarget))
            .Cast<TTarget> ()
            .First (x => (string)property?.GetValue (x)! == propertyName);
    }

    /// <summary>
    ///     Gets a property of type <typeparamref name="T"/> from a collection of type <see cref="object"/>.
    /// </summary>
    /// <param name="collection">The collection of type <see cref="object"/> to get the property from.</param>
    /// <param name="propertyName">The name of the property to append.</param>
    /// <typeparam name="T">The type of the property being appended.</typeparam>
    /// <returns>A property of type <typeparamref name="T"/>.</returns>
    public static T GetProperty<T> (this IEnumerable<object> collection, string propertyName)
    {
        var property = typeof (T).GetProperty ("PropertyName");

        return collection.Where (x => x?.GetType () == typeof (T))
            .Cast<T> ()
            .First (x => (string)property?.GetValue (x)! == propertyName);
    }
    #endregion
}
