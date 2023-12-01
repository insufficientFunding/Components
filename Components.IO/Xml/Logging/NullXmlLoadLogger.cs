using Microsoft.Extensions.Logging;
namespace Components.IO.Xml.Logging;

internal class NullXmlLoadLogger : IXmlLoadLogger
{
    public void Log (LogLevel level, FileRange position, string message, Exception? innerException = null)
    {
        // Do nothing
    }
}
