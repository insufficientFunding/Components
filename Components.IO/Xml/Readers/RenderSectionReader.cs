using Autofac.Features.Indexed;
using Components.Base.Primitives;
using Components.IO.Xml.Flatten;
using Components.IO.Xml.Interfaces;
using Components.IO.Xml.Logging;
using Components.IO.Xml.Parsers.ComponentPoints;
using Components.IO.Xml.Parsers.Conditions;
using Components.IO.Xml.Primitives;
using Components.IO.Xml.Readers.RenderCommands;
using Components.IO.Xml.Render;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.Render.TypeDescription.TypeDescription;
using System.Xml.Linq;
namespace Components.IO.Xml.Readers;

internal class RenderSectionReader : IXmlSectionReader
{
    private readonly IXmlLoadLogger _logger;
    private readonly IConditionParser _conditionParser;
    private readonly IAttributeParser _attributeParser;
    private readonly IIndex<string, IRenderCommandReader> _renderCommandReaders;
    private readonly IComponentPointParser _componentPointParser;
    private readonly IAutoRotateOptionsReader _autoRotateOptionsReader;


    public RenderSectionReader (
        IXmlLoadLogger logger,
        IConditionParser conditionParser,
        IAttributeParser attributeParser,
        IIndex<string, IRenderCommandReader> renderCommandReaders,
        IComponentPointParser componentPointParser,
        IAutoRotateOptionsReader autoRotateOptionsReader)
    {
        _logger = logger;
        _conditionParser = conditionParser;
        _attributeParser = attributeParser;
        _renderCommandReaders = renderCommandReaders;
        _componentPointParser = componentPointParser;
        _autoRotateOptionsReader = autoRotateOptionsReader;
    }

    public void ReadSection (XElement element, ComponentDescription description)
    {
        List<XmlRenderGroup> groups = [];
        XmlRenderGroup defaultGroup = new XmlRenderGroup (ConditionTree.Empty);

        _autoRotateOptionsReader.TrySetAutoRotateOptions (element, defaultGroup);

        description.Configuration.AutoRotate = ((IAutoRotateRoot)defaultGroup).AutoRotate;
        description.Configuration.AutoRotateFlip = ((IAutoRotateRoot)defaultGroup).AutoRotateFlip;

        groups.Add (defaultGroup);
        foreach (XElement child in element.Elements ())
            groups.AddRange (ReadElement (child, description, defaultGroup));

        RenderDescription [] flatGroups = groups.SelectMany (x => x.FlattenRoot ()).ToArray ();
        ((ComponentDescription)description).RenderDescriptions = flatGroups
            .GroupBy (x => ConditionsReducer.SimplifyConditions (x.Conditions))
            .Select (g => new RenderDescription (g.Key, g
                                                     .SelectMany (x => x.Value)
                                                     .ToArray ())).ToArray ();
    }

    protected virtual IEnumerable<XmlRenderGroup> ReadElement (XElement element, ComponentDescription description, XmlRenderGroup groupContext)
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

    protected virtual IEnumerable<XmlRenderGroup> ReadRenderGroup (ComponentDescription description, XElement renderElement, XmlRenderGroup parentGroup)
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

        if (_attributeParser.ParseDouble (element, "Thickness", _logger, out double? value, 1D))
            command.Thickness = (double)value!;

        if (!_componentPointParser.TryParse (element.Attribute ("Start")!, out XmlComponentPoint start))
            return _logger.LogErrorReturnFalse (element, $"Invalid position value '{start}'");

        command.Start = start;

        if (!_componentPointParser.TryParse (element.Attribute ("End")!, out XmlComponentPoint end))
            return _logger.LogErrorReturnFalse (element, $"Invalid position value '{start}'");

        command.End = end;

        return true;
    }

    protected virtual bool ReadEllipse (XElement element, out XmlEllipseCommand command)
    {
        command = new XmlEllipseCommand ();

        if (_attributeParser.ParseDouble (element, "Thickness", _logger, out double? value, 1D))
            command.StrokeThickness = (double)value!;

        XAttribute? fill = element.Attribute ("Fill");
        if (fill != null && fill.Value.ToLowerInvariant () != "false")
            command.Fill = true;

        XAttribute? Position = element.Attribute ("Position");
        if (Position != null)
        {
            if (!_componentPointParser.TryParse (Position, out XmlComponentPoint position))
                return _logger.LogErrorReturnFalse (element, $"Invalid position value '{position!}'");

            command.Position = position;
        }
        else
            return false;

        if (element.GetAttributeValue ("Radius", _logger, out string? radius))
        {
            if (!Size.TryParse (radius!, out Size? radii))
                return _logger.LogErrorReturnFalse (element, $"Invalid radius value '{radius}'");

            command.Size = radii!;
        }
        else
            return false;

        return true;
    }

    protected virtual bool ReadRectangleCommand (XElement element, out XmlRectangleCommand command)
    {
        command = new XmlRectangleCommand ();

        if (_attributeParser.ParseDouble (element, "Thickness", _logger, out double? value, 1D))
            command.StrokeThickness = (double)value!;

        XAttribute? fill = element.Attribute ("Fill");
        if (fill != null && fill.Value.ToLowerInvariant () != "false")
            command.Fill = true;

        if (element.GetAttribute ("Position", _logger, out XAttribute? pos))
        {
            if (!_componentPointParser.TryParse (pos!, out XmlComponentPoint position))
                return _logger.LogErrorReturnFalse (element, $"Invalid position value '{pos!.Value}'");

            command.Position = position;
        }
        else
            return false;

        if (element.GetAttributeValue ("Size", _logger, out string? sizeString))
        {
            if (!Size.TryParse (sizeString!, out Size? size))
                return _logger.LogErrorReturnFalse (element, $"Invalid size value '{size}'");

            command.Size = size!;
        }
        else
            return false;

        return true;
    }

    protected virtual bool ReadPathCommand (XElement element, out XmlRenderPath command)
    {
        command = new XmlRenderPath ();

        if (_attributeParser.ParseDouble (element, "Thickness", _logger, out double? value, 1D))
            command.Thickness = (double)value!;

        XAttribute? fill = element.Attribute ("Fill");
        if (fill != null && fill.Value.ToLowerInvariant () != "false")
            command.Fill = true;

        if (element.GetAttribute ("Position", _logger, out XAttribute? pos))
        {
            if (!_componentPointParser.TryParse (pos!, out XmlComponentPoint position))
                return _logger.LogErrorReturnFalse (element, $"Invalid position value '{pos!.Value}'");

            command.Start = position;
        }
        else
            return false;

        command.Commands = PathReader.ParseCommands (element, _logger);

        return true;
    }
}
