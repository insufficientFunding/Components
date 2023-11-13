using Components.DataModels;
using Components.Enums;
using Components.Primitives;
using System.Globalization;
namespace Components.Render.TypeDescription;

public class ComponentPoint
{
    public ComponentPosition RelativeToX { get; set; }

    public ComponentPosition RelativeToY { get; set; }


    public Point Offset { get; set; }

    public ComponentPoint ()
    {
        RelativeToX = ComponentPosition.Start;
        RelativeToY = ComponentPosition.Start;
        Offset = new Point ();
    }

    public ComponentPoint (
        ComponentPosition relativeToX,
        ComponentPosition relativeToY,
        Point offset)
    {
        RelativeToX = relativeToX;
        RelativeToY = relativeToY;
        Offset = offset;
    }

    public ComponentPoint Flip (FlipType type, FlipState flipState)
    {
        if (type == FlipType.None)
            return this;

        ComponentPoint? returnPoint = new ComponentPoint (RelativeToX, RelativeToY, Offset);
        if (type == FlipType.Horizontal)
            returnPoint.Offset = new Point (-Offset.X, Offset.Y);
        else if (type == FlipType.Vertical)
            returnPoint.Offset = new Point (Offset.X, -Offset.Y);
        else if (type == FlipType.Both)
            returnPoint.Offset = new Point (-Offset.X, -Offset.Y);

        if (flipState == FlipState.Secondary)
            return returnPoint;

        if (RelativeToX == ComponentPosition.Start)
            returnPoint.RelativeToX = ComponentPosition.End;
        else if (RelativeToX == ComponentPosition.Middle)
            returnPoint.RelativeToX = ComponentPosition.Middle;
        else if (RelativeToX == ComponentPosition.End)
            returnPoint.RelativeToX = ComponentPosition.Start;

        if (RelativeToY == ComponentPosition.Start)
            returnPoint.RelativeToY = ComponentPosition.End;
        else if (RelativeToY == ComponentPosition.Middle)
            returnPoint.RelativeToY = ComponentPosition.Middle;
        else if (RelativeToY == ComponentPosition.End)
            returnPoint.RelativeToY = ComponentPosition.Start;

        return returnPoint;
    }

    public Point Resolve (LayoutInformation layout)
    {
        FlipType flipType = layout.GetFlipType ();
        ComponentPoint tempPoint = Flip (flipType, layout.Flip);

        double x = tempPoint.Offset.X;
        double y = tempPoint.Offset.Y;

        if (tempPoint.RelativeToX == ComponentPosition.Middle)
            x += layout.Size / 2;
        if (tempPoint.RelativeToY == ComponentPosition.Middle)
            y += layout.Size / 2;

        if (tempPoint.RelativeToX == ComponentPosition.End)
            x += layout.Size;
        if (tempPoint.RelativeToY == ComponentPosition.End)
            y += layout.Size;

        return new Point (x, y);
    }

    public override string ToString ()
    {
        string xOffset = Offset.X.ToString (CultureInfo.InvariantCulture);
        if (Offset.X >= 0)
            xOffset = "+" + Offset.X.ToString (CultureInfo.InvariantCulture);
        string yOffset = Offset.Y.ToString (CultureInfo.InvariantCulture);
        if (Offset.Y >= 0)
            yOffset = "+" + Offset.Y.ToString (CultureInfo.InvariantCulture);

        return string.Format ("{0}{1},{2}{3}", RelativeToX, RelativeToY, xOffset, yOffset);
    }

    public override bool Equals (object? obj)
    {
        ComponentPoint? other = obj as ComponentPoint;
        if (other == null)
            return false;

        return other.Offset == Offset && other.RelativeToX == RelativeToX && other.RelativeToY == RelativeToY;
    }

    protected bool Equals (ComponentPoint other)
    {
        return RelativeToX == other.RelativeToX && RelativeToY == other.RelativeToY && Offset.Equals (other.Offset);
    }

    public override int GetHashCode ()
    {
        return HashCode.Combine ((int)RelativeToX, (int)RelativeToY, Offset);
    }
}
public enum ComponentPosition
{
    Absolute = 0,
    Start = 1,
    Middle = 2,
    End = 3,
}
