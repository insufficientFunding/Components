using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
namespace Components.VisualEditor.Logging;

internal static class LoggingSetup
{
    public static ILoggingBuilder SetupLogging (this ILoggingBuilder builder, bool verbose, bool silent)
    {
        builder.AddConsole (x => x.FormatterName = "BasicConsoleFormatter").AddConsoleFormatter<BasicConsoleFormatter, BasicConsoleFormatterOptions> ().SetMinimumLevel (verbose ? LogLevel.Debug : LogLevel.Information);
        return builder;
    }
}
internal class BasicConsoleFormatterOptions : ConsoleFormatterOptions
{ }
