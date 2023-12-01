using Components.Base.Enums;
using Components.IO.Xml.Flatten;
using Components.IO.Xml.Logging;
using System.Xml.Linq;
namespace Components.IO.Xml.Readers;

internal class AutoRotateOptionsReader : IAutoRotateOptionsReader
{
    private readonly IXmlLoadLogger _logger;

    public AutoRotateOptionsReader (IXmlLoadLogger logger)
    {
        _logger = logger;
    }

    public bool TrySetAutoRotateOptions (XElement element, IAutoRotateRoot target)
    {
        return TrySetAutoRotateOptions (element, null, target);
    }
    public bool TrySetAutoRotateOptions (XElement element, IAutoRotateRoot? ancestor, IAutoRotateRoot target)
    {
        if (ancestor != null)
        {
            target.AutoRotate = ancestor.AutoRotate;
            target.AutoRotateFlip = ancestor.AutoRotateFlip;
        }

        XAttribute? autorotate = element.Attribute ("AutoRotate");
        if (autorotate == null)
            return true;

        string []? options = autorotate.Value.Replace (" ", string.Empty).Split (',');

        if (options.Length == 0)
        {
            _logger.LogError (autorotate, "Autorotate options cannot be empty");
            return false;
        }

        if (!Enum.TryParse (options [0], true, out AutoRotateType autoRotateType))
        {
            _logger.LogError (autorotate, $"Unknown autorotation type '{options [0]}'");
            return false;
        }

        FlipState flipState = FlipState.None;
        foreach (string option in options.Skip (1))
        {
            if (string.Equals (option, "FlipPrimary", StringComparison.OrdinalIgnoreCase))
                flipState |= FlipState.Primary;
            else if (string.Equals (option, "FlipSecondary", StringComparison.OrdinalIgnoreCase))
                flipState |= FlipState.Secondary;
            else
            {
                _logger.LogError (autorotate, $"Unknown autorotation option '{option}'");
                return false;
            }
        }

        target.AutoRotate = autoRotateType;
        target.AutoRotateFlip = flipState;
        return true;
    }
}
