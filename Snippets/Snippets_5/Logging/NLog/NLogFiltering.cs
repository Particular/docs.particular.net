using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;

public class NLogFiltering
{
    public void Filtering()
    {
        #region NLogFiltering

        LoggingConfiguration config = new LoggingConfiguration();

        ColoredConsoleTarget target = new ColoredConsoleTarget();
        config.AddTarget("console", target);
        config.LoggingRules.Add(new LoggingRule("MyNamespace.*", LogLevel.Debug, target));

        LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.Use<NLogFactory>();

        #endregion
    }
}