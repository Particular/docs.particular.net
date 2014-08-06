using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus.NLog;

public class NLogConfig
{
    public void InCode()
    {
        #region NLogInCode

        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level:uppercase=true}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

        LogManager.Configuration = config;
        NLogConfigurator.Configure();

        #endregion
    }
}