using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;

namespace NLog_1
{
    public class NLogFiltering
    {
        public NLogFiltering()
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
}
