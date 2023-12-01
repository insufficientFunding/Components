using Microsoft.Extensions.Logging;
using System.Text;
namespace Components.IO.Xml.Logging;

internal class XmlLoadLogger : IXmlLoadLogger
{
    private readonly ILogger logger;
    private readonly string fileName;

    public XmlLoadLogger (ILogger logger, string fileName)
    {
        this.logger = logger;
        this.fileName = fileName.Replace (Environment.CurrentDirectory, ".") ?? "File.xml";
    }

    public void Log (LogLevel level, FileRange position, string message, Exception? innerException = null)
    {
        logger.Log (level, new EventId (), (object)null!, innerException, (_, ex) =>
        {
            StringBuilder? builder = new StringBuilder ();
            builder.Append ($"{level.ToString ().ToUpperInvariant ()} {fileName}({position.StartLine},{position.StartCol}:{position.EndLine},{position.EndCol}): {message}");

            if (ex is not null)
            {
                builder.AppendLine ();
                builder.Append ((object?)ex);
            }

            return builder.ToString ();
        });
    }
}
