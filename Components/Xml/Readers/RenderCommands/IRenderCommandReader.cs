﻿using Components.Interfaces.TypeDescription;
using Components.Xml.Render;
using System.Xml.Linq;
namespace Components.Xml.Readers.RenderCommands;

/// <summary>
///     Represents a reader for render commands.
/// </summary>
internal interface IRenderCommandReader
{
    /// <summary>
    ///     Reads a render command from the given element.
    /// </summary>
    /// <param name="element">The <see cref="XElement"/> to read from.</param>
    /// <param name="description">The component description.</param>
    /// <param name="command">The output render command.</param>
    /// <returns><b>True</b> if the command was read successfully, <b>false</b> otherwise.</returns>
    bool ReadRenderCommand (XElement element, IComponentDescription description, out IXmlRenderCommand command);
}
