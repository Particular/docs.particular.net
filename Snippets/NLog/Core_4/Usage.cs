using global::NLog;
using global::NLog.Config;
using global::NLog.Targets;
using NServiceBus;

class Usage
{
    Usage()
    {
        #region NLogInCode

        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

        LogManager.Configuration = config;

        SetLoggingLibrary.NLog();

        #endregion
    }
}