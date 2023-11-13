using Microsoft.Extensions.Logging;
namespace Components.Logging;

public static class ILoggerExtensions
{
    #region Information
    /// <summary>
    ///     Logs a message and returns true.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="message">The message.</param>
    /// <param name="args">An array of zero or more objects to format.</param>
    /// <returns><c>true</c></returns>
    public static bool LogInformationReturnTrue (this ILogger logger, string? message, params object? [] args)
    {
        logger.LogInformation (message, args);

        return true;
    }

    /// <summary>
    ///     Logs a message and returns false.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="message">The message.</param>
    /// <param name="args">An array of zero or more objects to format.</param>
    /// <returns><c>false</c></returns>
    public static bool LogInformationReturnFalse (this ILogger logger, string? message, params object? [] args)
    {
        logger.LogInformation (message, args);

        return false;
    }

    /// <summary>
    ///     Logs a message and returns the default value of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="message">The message.</param>
    /// <param name="args">An array of zero or more objects to format.</param>
    /// <typeparam name="T">The type of the default value.</typeparam>
    /// <returns>The default value of <typeparamref name="T"/>.</returns>
    public static T LogInformationReturnDefault<T> (this ILogger logger, string? message, params object? [] args)
    {
        logger.LogInformation (message, args);

        return default!;
    }
  #endregion

    #region Errors
    /// <summary>
    ///     Logs an error and returns true.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="message">The message.</param>
    /// <param name="args">An array of zero or more objects to format.</param>
    /// <returns><c>true</c></returns>
    public static bool LogErrorReturnTrue (this ILogger logger, string? message, params object? [] args)
    {
        logger.LogError (message, args);

        return true;
    }

    /// <summary>
    ///     Logs an error and returns false.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="message">The message.</param>
    /// <param name="args">An array of zero or more objects to format.</param>
    /// <returns><c>false</c></returns>
    public static bool LogErrorReturnFalse (this ILogger logger, string? message, params object? [] args)
    {
        logger.LogError (message, args);

        return false;
    }

    /// <summary>
    ///     Logs an error and returns the default value of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="message">The message.</param>
    /// <param name="args">An array of zero or more objects to format.</param>
    /// <typeparam name="T">The type of the default value.</typeparam>
    /// <returns>The default value of <typeparamref name="T"/>.</returns>
    public static T LogErrorReturnDefault<T> (this ILogger logger, string? message, params object? [] args)
    {
        logger.LogError (message, args);

        return default!;
    }
    #endregion
}
