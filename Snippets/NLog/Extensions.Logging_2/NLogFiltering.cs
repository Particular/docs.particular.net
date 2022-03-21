using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NServiceBus.Extensions.Logging;

public class NLogFiltering
{
    public NLogFiltering()
    {
        #region NLogFiltering

        var config = new LoggingConfiguration();

        var target = new ColoredConsoleTarget();
        config.AddTarget("console", target);
        config.LoggingRules.Add(new LoggingRule("MyNamespace.*", LogLevel.Debug, target));

        LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.UseFactory(new ExtensionsLoggerFactory(new NLogLoggerFactory()));

        #endregion
    }
}