using Autofac.Features.Indexed;
using Components.Extensions;
using Components.Interfaces.Conditions;
using Components.Interfaces.TypeDescription;
using Components.Primitives;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.Render.TypeDescription.TypeDescription;
using Components.Xml.Flatten;
using Components.Xml.Logging;
using Components.Xml.Parsers.ComponentPoints;
using Components.Xml.Parsers.Conditions;
using Components.Xml.Primitives;
using Components.Xml.Readers.RenderCommands;
using Components.Xml.Render;
using System.Xml.Linq;
namespace Components.Xml.Readers;

internal class RenderSectionReader : IXmlSectionReader
{
    private readonly IXmlLoadLogger _logger;
    private readonly IConditionParser _conditionParser;
    private readonly IIndex<string, IRenderCommandReader> _renderCommandReaders;
    private readonly IComponentPointParser _componentPointParser;
    private readonly IAutoRotateOptionsReader _autoRotateOptionsReader;


    public RenderSectionReader (
        IXmlLoadLogger logger,
        IConditionParser conditionParser,
        IIndex<string, IRenderCommandReader> renderCommandReaders,
        IComponentPointParser componentPointParser,
        IAutoRotateOptionsReader autoRotateOptionsReader)
    {
        _logger = logger;
        _conditionParser = conditionParser;
        _renderCommandReaders = renderCommandReaders;
        _componentPointParser = componentPointParser;
        _autoRotateOptionsReader = autoRotateOptionsReader;
    }

    public void ReadSection (XElement element, IComponentDescription description)
    {
        List<XmlRenderGroup> groups = new List<XmlRenderGroup> ();
        XmlRenderGroup defaultGroup = new XmlRenderGroup (ConditionTree.Empty);

        _autoRotateOptionsReader.TrySetAutoRotateOptions (element, defaultGroup);

        ((ComponentConfiguration)description.Configuration).AutoRotate = ((IAutoRotateRoot)defaultGroup).AutoRotate;
        ((ComponentConfiguration)description.Configuration).AutoRotateFlip = ((IAutoRotateRoot)defaultGroup).AutoRotateFlip;

        groups.Add (defaultGroup);
        foreach (XElement child in element.Elements ())
            groups.AddRange (ReadElement (child, description, defaultGroup));

        IRenderDescription [] flatGroups = groups.SelectMany (x => x.FlattenRoot ()).ToArray ();
        ((ComponentDescription)description).RenderDescriptions = flatGroups
            .GroupBy (x => ConditionsReducer.SimplifyConditions (x.Conditions))
            .Select (g => new RenderDescription (g.Key, g
                                                     .SelectMany (
                                                     x => x.Value)
                                                     .ToArray ())).ToArray ();
    }

    protected virtual IEnumerable<XmlRenderGroup> ReadElement (XElement element, IComponentDescription description, XmlRenderGroup groupContext)
    {
        switch (element.Name.LocalName)
        {
            case "RenderGroup":
                return ReadRenderGroup (description, element, groupContext);
            case "Line":
                if (ReadLineCommand (element, out XmlLineCommand line))
                    groupContext.Value.Add (line);
                return Enumerable.Empty<XmlRenderGroup> ();
            case "Ellipse":
                if (ReadEllipse (element, out XmlEllipseCommand ellipse))
                    groupContext.Value.Add (ellipse);
                return Enumerable.Empty<XmlRenderGroup> ();
            case "Rectangle":
                if (ReadRectangleCommand (element, out XmlRectangleCommand rectangle))
                    groupContext.Value.Add (rectangle);
                return Enumerable.Empty<XmlRenderGroup> ();
            case "Path":
                if (ReadPathCommand (element, out XmlRenderPath path))
                    groupContext.Value.Add (path);
                return Enumerable.Empty<XmlRenderGroup> ();
            case "Text":
                if (!_renderCommandReaders.TryGetValue ($"{XmlLoader.ComponentNamespace.NamespaceName}Text", out IRenderCommandReader textReader))
                    Console.Error.WriteLine ($"No reader found for {element.Name.LocalName}");
                if (textReader.ReadRenderCommand (element, description, out IXmlRenderCommand command))
                    groupContext.Value.Add (command);
                return Enumerable.Empty<XmlRenderGroup> ();
            default:
                return Enumerable.Empty<XmlRenderGroup> ();
        }
    }

    protected virtual IEnumerable<XmlRenderGroup> ReadRenderGroup (IComponentDescription description, XElement renderElement, XmlRenderGroup parentGroup)
    {
        IConditionTreeItem conditionCollection = ConditionTree.Empty;
        XAttribute? conditionsAttribute = renderElement.Attribute ("Conditions");
        if (conditionsAttribute != null)
        {
            if (!_conditionParser.Parse (conditionsAttribute, description, out conditionCollection))
                yield break;
        }

        XmlRenderGroup renderGroup = new XmlRenderGroup (new ConditionTree (ConditionTree.ConditionOperator.AND, parentGroup.Conditions, conditionCollection))
        {
            AutoRotate = parentGroup.AutoRotate,
            AutoRotateFlip = parentGroup.AutoRotateFlip,
        };

        _autoRotateOptionsReader.TrySetAutoRotateOptions (renderElement, renderGroup);

        IEnumerable<XmlRenderGroup> childGroups = renderElement.Elements ().SelectMany (x => ReadElement (x, description, renderGroup));

        yield return renderGroup;
        foreach (XmlRenderGroup child in childGroups)
            yield return child;
    }

    protected virtual bool ReadLineCommand (XElement element, out XmlLineCommand command)
    {
        command = new XmlLineCommand ();

        if (element.Attribute ("Thickness") != null)
            command.Thickness = element.Attribute ("Thickness")!.Value.ParseDecimal ();

        if (!_componentPointParser.TryParse (element.Attribute ("Start")!, out XmlComponentPoint start))
            return _logger.LogErrorReturn (element, $"Invalid position value '{start}'");

        command.Start = start;

        if (!_componentPointParser.TryParse (element.Attribute ("End")!, out XmlComponentPoint end))
            return _logger.LogErrorReturn (element, $"Invalid position value '{start}'");

        command.End = end;

        return true;
    }

    protected virtual bool ReadEllipse (XElement element, out XmlEllipseCommand command)
    {
        command = new XmlEllipseCommand ();

        if (element.Attribute ("Thickness") != null)
            command.StrokeThickness = element.Attribute ("Thickness")!.Value.ParseDecimal ();

        XAttribute? fill = element.Attribute ("Fill");
        if (fill != null && fill.Value.ToLowerInvariant () != "false")
            command.Fill = true;

        XAttribute? Position = element.Attribute ("Position");
        if (Position != null)
        {
            if (!_componentPointParser.TryParse (Position, out XmlComponentPoint position))
                return _logger.LogErrorReturn (element, $"Invalid position value '{position!}'");

            command.Position = position;
        }
        else
            return false;

        if (element.GetAttributeValue ("Radius", _logger, out string? radius))
        {
            if (!Size.TryParse (radius!, out Size? radii))
                return _logger.LogErrorReturn (element, $"Invalid radius value '{radius}'");

            command.Size = radii!;
        }
        else
            return false;

        return true;
    }

    protected virtual bool ReadRectangleCommand (XElement element, out XmlRectangleCommand command)
    {
        command = new XmlRectangleCommand ();

        if (element.Attribute ("Thickness") != null)
            command.StrokeThickness = element.Attribute ("Thickness")!.Value.ParseDecimal ();

        XAttribute? fill = element.Attribute ("Fill");
        if (fill != null && fill.Value.ToLowerInvariant () != "false")
            command.Fill = true;

        if (element.GetAttribute ("Position", _logger, out XAttribute? pos))
        {
            if (!_componentPointParser.TryParse (pos!, out XmlComponentPoint position))
                return _logger.LogErrorReturn (element, $"Invalid position value '{pos!.Value}'");

            command.Position = position;
        }
        else
            return false;

        if (element.GetAttributeValue ("Size", _logger, out string? sizeString))
        {
            if (!Size.TryParse (sizeString!, out Size? size))
                return _logger.LogErrorReturn (element, $"Invalid size value '{size}'");

            command.Size = size!;
        }
        else
            return false;

        return true;
    }

    protected virtual bool ReadPathCommand (XElement element, out XmlRenderPath command)
    {
        command = new XmlRenderPath ();

        if (element.Attribute ("Thickness") != null)
            command.Thickness = element.Attribute ("Thickness")!.Value.ParseDecimal ();

        XAttribute? fill = element.Attribute ("Fill");
        if (fill != null && fill.Value.ToLowerInvariant () != "false")
            command.Fill = true;

        if (element.GetAttribute ("Position", _logger, out XAttribute? pos))
        {
            if (!_componentPointParser.TryParse (pos!, out XmlComponentPoint position))
                return _logger.LogErrorReturn (element, $"Invalid position value '{pos!.Value}'");

            command.Start = position;
        }
        else
            return false;

        command.Commands = PathReader.ParseCommands (element, _logger);

        return true;
    }
}
