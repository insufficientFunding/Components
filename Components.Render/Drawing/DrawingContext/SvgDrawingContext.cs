using Components.Base.Enums;
using Components.Base.Primitives;
using Components.Render.Drawing.RenderCommands.Path;
using Components.Render.Text;
using System.Globalization;
using System.Text;
using System.Xml;
namespace Components.Render.Drawing.DrawingContext;

public class SvgDrawingContext : IDrawingContext
{
    private readonly double width;
    private readonly double height;
    private readonly XmlWriter writer;
    
    public double Thickness { get; set; } = 0.1;

    public SvgDrawingContext (double width, double height, Stream output)
    {
        writer = XmlWriter.Create (output, new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            IndentChars = "\t",
        });
        this.width = width;
        this.height = height;

        Begin ();
    }

    private void Begin ()
    {
        writer.WriteStartDocument ();
        writer.WriteStartElement ("svg", "http://www.w3.org/2000/svg");
        writer.WriteAttributeString ("version", "1.1");
        writer.WriteAttributeString ("width", width.ToString (CultureInfo.InvariantCulture));
        writer.WriteAttributeString ("height", height.ToString (CultureInfo.InvariantCulture));
    }

    private void End ()
    {
        writer.WriteEndDocument ();
        writer.Flush ();
    }

    public void DrawLine (Point start, Point end, double thickness)
    {
        thickness = thickness * Thickness;
        
        writer.WriteStartElement ("line");

        writer.WriteAttributeString ("x1", start.X.ToString (CultureInfo.InvariantCulture));
        writer.WriteAttributeString ("y1", start.Y.ToString (CultureInfo.InvariantCulture));
        writer.WriteAttributeString ("x2", end.X.ToString (CultureInfo.InvariantCulture));
        writer.WriteAttributeString ("y2", end.Y.ToString (CultureInfo.InvariantCulture));

        writer.WriteAttributeString ("style", "stroke:rgb(255, 255, 255);stroke-linecap:square;stroke-width:" + thickness.ToString (CultureInfo.InvariantCulture));

        writer.WriteEndElement ();
    }

    public void DrawRectangle (Point start, Size size, double thickness, bool fill = false)
    {
        thickness = thickness * Thickness;
        
        string fillOpacity = ((fill ? 255f : 0f) / 255f).ToString (CultureInfo.InvariantCulture);

        writer.WriteStartElement ("rect");

        writer.WriteAttributeString ("x", start.X.ToString (CultureInfo.InvariantCulture));
        writer.WriteAttributeString ("y", start.Y.ToString (CultureInfo.InvariantCulture));

        writer.WriteAttributeString ("width", size.Width.ToString (CultureInfo.InvariantCulture));
        writer.WriteAttributeString ("height", size.Height.ToString (CultureInfo.InvariantCulture));

        writer.WriteAttributeString ("style", $"fill-opacity:{fillOpacity};fill:rgb(255, 255, 255);stroke:rgb(255, 255, 255);stroke-width:{thickness.ToString (CultureInfo.InvariantCulture)}");

        writer.WriteEndElement ();
    }

    public void DrawEllipse (Point center, Size radius, double thickness, bool fill = false)
    {
        thickness = thickness * Thickness;
        
        string fillOpacity = ((fill ? 255f : 0f) / 255f).ToString (CultureInfo.InvariantCulture);

        writer.WriteStartElement ("ellipse");

        writer.WriteAttributeString ("cx", center.X.ToString (CultureInfo.InvariantCulture));
        writer.WriteAttributeString ("cy", center.Y.ToString (CultureInfo.InvariantCulture));

        writer.WriteAttributeString ("rx", radius.Width.ToString (CultureInfo.InvariantCulture));
        writer.WriteAttributeString ("ry", radius.Height.ToString (CultureInfo.InvariantCulture));

        writer.WriteAttributeString ("style", $"fill-opacity:{fillOpacity};fill:rgb(255, 255, 255);stroke:rgb(255, 255, 255);stroke-width:{thickness.ToString (CultureInfo.InvariantCulture)}");

        writer.WriteEndElement ();
    }

    public void DrawPath (Point start, List<IPathCommand> commands, double thickness, bool fill = false)
    {
        thickness = thickness * Thickness;
        
        string x = start.X.ToString (CultureInfo.InvariantCulture);
        string y = start.Y.ToString (CultureInfo.InvariantCulture);
        
        string data = string.Empty;
        data += $"M {x}, {y} ";

        foreach (IPathCommand pathCommand in commands)
        {
            data += " " + pathCommand.Shorthand ();
        }

        string fillOpacity = ((fill ? 255f : 0f) / 255f).ToString (CultureInfo.InvariantCulture);

        writer.WriteStartElement ("path");

        writer.WriteAttributeString ("d", data);
        writer.WriteAttributeString ("style", $"fill-opacity:{fillOpacity};fill:rgb(255, 255, 255);stroke:rgb(255, 255, 255);stroke-width:{thickness.ToString (CultureInfo.InvariantCulture)}");

        writer.WriteEndElement ();
    }

    public void DrawText (Point anchor, TextAlignment alignment, FontWeight weight, double rotation, IList<TextRun> textRuns)
    {
        
    }

    public void Dispose ()
    {
        End ();
    }
}

