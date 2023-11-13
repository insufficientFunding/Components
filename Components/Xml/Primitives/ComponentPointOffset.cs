namespace Components.Xml.Primitives;

internal class ComponentPointOffset
{
    public double Offset { get; set; }

    public OffsetAxis Axis { get; set; }
}
public enum OffsetAxis
{
    X,
    Y,
}
