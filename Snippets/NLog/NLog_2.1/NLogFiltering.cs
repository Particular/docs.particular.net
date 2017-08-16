using global::NLog;
using global::NLog.Config;
using global::NLog.Targets;
using NServiceBus;

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

        NServiceBus.Logging.LogManager.Use<NLogFactory>();

        #endregion
    }
}
