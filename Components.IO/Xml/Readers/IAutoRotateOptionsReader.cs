using Components.IO.Xml.Flatten;
using System.Xml.Linq;
namespace Components.IO.Xml.Readers;

internal interface IAutoRotateOptionsReader
{
    bool TrySetAutoRotateOptions (XElement element, IAutoRotateRoot target);

    bool TrySetAutoRotateOptions (XElement element, IAutoRotateRoot? ancestor, IAutoRotateRoot target);
}
