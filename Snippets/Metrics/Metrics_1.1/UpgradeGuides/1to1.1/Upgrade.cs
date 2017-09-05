using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Upgrade
{

    void EnableToTrace(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11EnableToTrace

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.RegisterObservers(context =>
        {
            foreach (var duration in context.Durations)
            {
                duration.Register(durationLength =>
                {
                    Trace.WriteLine($"Duration '{duration.Name}' value observed: '{durationLength}'");
                });
            }
            foreach (var signal in context.Signals)
            {
                signal.Register(() =>
                {
                    Trace.WriteLine($"'{signal.Name}' occurred.");
                });
            }
        });
        #endregion
    }

    void EnableToLog(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11EnableToLog

        //TODO: the logger instance should be a static field
        var logger = LogManager.GetLogger("LoggerName");

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.RegisterObservers(context =>
        {
            foreach (var duration in context.Durations)
            {
                duration.Register(durationLength =>
                {
                    logger.Info($"Duration '{duration.Name}' value observed: '{durationLength}'");
                });
            }
            foreach (var signal in context.Signals)
            {
                signal.Register(() =>
                {
                    logger.Info($"'{signal.Name}' occurred.");
                });
            }
        });

        #endregion
    }

    void Custom(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11Custom

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.RegisterObservers(context =>
        {
            foreach (var duration in context.Durations)
            {
                duration.Register(durationLength =>
                {
                    ProcessMetric(duration);
                });
            }
            foreach (var signal in context.Signals)
            {
                signal.Register(() =>
                {
                    ProcessMetric(signal);
                });
            }
        });

        #endregion
    }
    static Task ProcessMetric(IDurationProbe data)
    {
        return Task.CompletedTask;
    }
    static Task ProcessMetric(ISignalProbe data)
    {
        return Task.CompletedTask;
    }

}