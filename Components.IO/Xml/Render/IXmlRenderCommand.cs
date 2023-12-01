using Components.IO.Xml.Flatten;
using Components.Render.Drawing.RenderCommands;
namespace Components.IO.Xml.Render;

internal interface IXmlRenderCommand : IFlattenable<IRenderCommand>
{ }
