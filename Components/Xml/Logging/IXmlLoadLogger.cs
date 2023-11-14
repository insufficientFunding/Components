using Microsoft.Extensions.Logging;
using System.Xml.Linq;
namespace Components.Xml.Logging;

internal interface IXmlLoadLogger
{
    void Log (LogLevel level, FileRange position, string message, Exception? innerException = null);
}
internal static class XmlLoadLoggerExtensions
{
    public static void Log (this IXmlLoadLogger logger, LogLevel level, XElement? element, string message)
    {
        logger.Log (level, element!.GetFileRange (), message, null);
    }

    public static void Log (this IXmlLoadLogger logger, LogLevel level, XAttribute? attribute, string message)
    {
        logger.Log (level, attribute!.GetFileRange (), message, null);
    }

    public static void LogWarning (this IXmlLoadLogger logger, XElement? element, string message)
    {
        logger.Log (LogLevel.Warning, element, message);
    }

    public static void LogError (this IXmlLoadLogger logger, XElement? element, string message)
    {
        logger.Log (LogLevel.Error, element, message);
    }

    public static void LogWarning (this IXmlLoadLogger logger, XAttribute? attribute, string message)
    {
        logger.Log (LogLevel.Warning, attribute, message);
    }

    public static void LogError (this IXmlLoadLogger logger, XAttribute? attribute, string message)
    {
        logger.Log (LogLevel.Error, attribute, message);
    }

    public static bool LogErrorReturnFalse (this IXmlLoadLogger logger, XAttribute? attribute, string message)
    {
        logger.Log (LogLevel.Error, attribute, message);

        return false;
    }

    public static bool LogErrorReturnFalse (this IXmlLoadLogger logger, XElement? element, string message)
    {
        logger.Log (LogLevel.Error, element, message);

        return false;
    }

    public static bool LogReturn (this IXmlLoadLogger logger, bool returnBool, LogLevel logLevel, XAttribute? attribute, string message)
    {
        logger.Log (logLevel, attribute, message);

        return returnBool;
    }

    public static bool LogReturn (this IXmlLoadLogger logger, bool returnBool, LogLevel logLevel, XElement? element, string message)
    {
        logger.Log (logLevel, element, message);

        return returnBool;
    }
}
