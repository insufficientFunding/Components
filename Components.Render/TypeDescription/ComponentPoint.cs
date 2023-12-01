using Components.Base.DataModels;
using Components.Base.Enums;
using Components.Base.Primitives;
using System.Globalization;
namespace Components.Render.TypeDescription;

public class ComponentPoint
{
    public Anchor RelativeToX { get; set; }

    public Anchor RelativeToY { get; set; }


    public Point Offset { get; set; }

    public ComponentPoint ()
    {
        RelativeToX = Anchor.Start;
        RelativeToY = Anchor.Start;
        Offset = new Point ();
    }

    public ComponentPoint (
        Anchor relativeToX,
        Anchor relativeToY,
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

        if (RelativeToX == Anchor.Start)
            returnPoint.RelativeToX = Anchor.End;
        else if (RelativeToX == Anchor.Middle)
            returnPoint.RelativeToX = Anchor.Middle;
        else if (RelativeToX == Anchor.End)
            returnPoint.RelativeToX = Anchor.Start;

        if (RelativeToY == Anchor.Start)
            returnPoint.RelativeToY = Anchor.End;
        else if (RelativeToY == Anchor.Middle)
            returnPoint.RelativeToY = Anchor.Middle;
        else if (RelativeToY == Anchor.End)
            returnPoint.RelativeToY = Anchor.Start;

        return returnPoint;
    }

    public Point Resolve (LayoutInformation layout)
    {
        FlipType flipType = layout.GetFlipType ();
        ComponentPoint tempPoint = Flip (flipType, layout.Flip);

        double x = tempPoint.Offset.X;
        double y = tempPoint.Offset.Y;

        if (tempPoint.RelativeToX == Anchor.Middle)
            x += layout.Size / 2;
        if (tempPoint.RelativeToY == Anchor.Middle)
            y += layout.Size / 2;

        if (tempPoint.RelativeToX == Anchor.End)
            x += layout.Size;
        if (tempPoint.RelativeToY == Anchor.End)
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

    public enum Anchor
    {
        Absolute = 0,
        Start = 1,
        Middle = 2,
        End = 3,
    }
}
