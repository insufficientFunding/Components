using Components.Enums;
using Components.Interfaces.Render;
using Components.Primitives;
using System.Globalization;
namespace Components.Render.Drawing.RenderCommands.Path;

/// <inheritdoc cref="IPathCommand"/>
/// <summary>
///     Represents a command that draws an elliptical arc from the current cursor position to a specified point.
/// </summary>
public class EllipticalArcTo : IPathCommand
{
    public bool Relative { get; }
    
    public CommandType Type => CommandType.EllipticalArcTo;
    
    public Point End { get; }
    
    /// <summary>
    ///     Gets the radii of the arc, in the form of (rx, ry).
    /// </summary>
    public Point Radii { get; }
    
    /// <summary>
    ///     Gets the angle of the arc.
    /// </summary>
    public double Angle { get; }
    
    /// <summary>
    ///     Gets a boolean value indicating whether the arc is large or small.
    /// </summary>
    public bool IsLargeArc { get; }
    
    /// <summary>
    ///     Gets the sweep direction of the arc.
    /// </summary>
    public SweepDirection SweepDirection { get; }
    
    public EllipticalArcTo (Point radii, Point end, double angle, bool isLargeArc, SweepDirection sweepDirection, bool relative = true)
    {
        Relative = relative;
        Radii = radii;
        End = end;
        Angle = angle;
        IsLargeArc = isLargeArc;
        SweepDirection = sweepDirection;
    }

    public string Shorthand ()
    {
        string prefix = Relative ? "a " : "A ";

        return string.Format (
            prefix + "{0}, {1} {2} {3} {4} {5}, {6}",
            Radii.X.ToString (CultureInfo.InvariantCulture), Radii.Y.ToString (CultureInfo.InvariantCulture),
            Angle.ToString (CultureInfo.InvariantCulture),
            IsLargeArc ? 1 : 0,
            SweepDirection == SweepDirection.Clockwise ? "1" : "0",
            End.X.ToString (CultureInfo.InvariantCulture), End.Y.ToString (CultureInfo.InvariantCulture)
        );
    }

    public IPathCommand Flip (bool horizontal)
    {
        SweepDirection newSweepDirection = SweepDirection == SweepDirection.Clockwise ? SweepDirection.CounterClockwise : SweepDirection.Clockwise;

        if (horizontal)
            return new EllipticalArcTo (Radii, new Point (-End.X, End.Y), Angle, IsLargeArc, newSweepDirection);

        return new EllipticalArcTo (Radii, new Point (End.X, -End.Y), Angle, IsLargeArc, newSweepDirection);
    }

    public IPathCommand Reflect ()
    {
        SweepDirection newSweepDirection = SweepDirection == SweepDirection.Clockwise ? SweepDirection.CounterClockwise : SweepDirection.Clockwise;

        return new EllipticalArcTo (new Point (Radii.Y, Radii.X), new Point (End.Y, End.X), Angle, IsLargeArc, newSweepDirection);
    }
}