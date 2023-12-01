using Microsoft.Extensions.Logging;
namespace Components.IO.Xml.Logging;

internal class ErrorCheckingLogger : IXmlLoadLogger
{
    private readonly IXmlLoadLogger _underlying;

    public ErrorCheckingLogger (IXmlLoadLogger underlying)
    {
        _underlying = underlying;
    }

    public bool HasErrors { get; private set; }

    public void Log (LogLevel level, FileRange position, string message, Exception? innerException = null)
    {
        if (level >= LogLevel.Error)
            HasErrors = true;

        _underlying.Log (level, position, message, innerException);
    }
}
