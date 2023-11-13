using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
namespace Components.Logging;

internal class BasicConsoleFormatter : ConsoleFormatter
{
    public BasicConsoleFormatter () : base (nameof (BasicConsoleFormatter))
    { }

    public override void Write<TState> (in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        textWriter.WriteLine (logEntry.Formatter (logEntry.State, logEntry.Exception));
    }
}
