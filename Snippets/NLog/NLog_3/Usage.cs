using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;

class Usage
{
    Usage()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        #region NLogInCode

        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

        LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.Use<NLogFactory>();

        #endregion
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}
