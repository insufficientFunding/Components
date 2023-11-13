using Components.Xml.Flatten;
using System.Xml.Linq;
namespace Components.Xml.Readers;

internal interface IAutoRotateOptionsReader
{
    bool TrySetAutoRotateOptions (XElement element, IAutoRotateRoot target);

    bool TrySetAutoRotateOptions (XElement element, IAutoRotateRoot? ancestor, IAutoRotateRoot target);
}
