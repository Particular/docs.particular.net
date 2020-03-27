using global::NLog;
using global::NLog.Config;
using global::NLog.Targets;
using NServiceBus;

public class NLogFiltering
{
    public NLogFiltering()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        #region NLogFiltering

        var config = new LoggingConfiguration();

        var target = new ColoredConsoleTarget();
        config.AddTarget("console", target);
        config.LoggingRules.Add(new LoggingRule("MyNamespace.*", LogLevel.Debug, target));

        LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.Use<NLogFactory>();

        #endregion
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}
