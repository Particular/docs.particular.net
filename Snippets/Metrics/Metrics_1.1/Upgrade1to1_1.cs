using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Upgrade1to1_1
{

    void EnableToTrace(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11EnableToTrace

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: length =>
                        {
                            Trace.WriteLine($"Duration '{duration.Name}'. Value: '{length}'");
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: () =>
                        {
                            Trace.WriteLine($"Signal: '{signal.Name}'");
                        });
                }
            });

        #endregion
    }

    void EnableToLog(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11EnableToLog

        //TODO: the logger instance should be a static field
        var log = LogManager.GetLogger("LoggerName");

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: length =>
                        {
                            log.Info($"Duration: '{duration.Name}'. Value: '{length}'");
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: () =>
                        {
                            log.Info($"Signal: '{signal.Name}'");
                        });
                }
            });

        #endregion
    }

    void Custom(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11Custom

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: length =>
                        {
                            ProcessMetric(duration, length);
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: () =>
                        {
                            ProcessMetric(signal);
                        });
                }
            });

        #endregion
    }

    static Task ProcessMetric(IDurationProbe data, TimeSpan length)
    {
        return Task.CompletedTask;
    }

    static Task ProcessMetric(ISignalProbe data)
    {
        return Task.CompletedTask;
    }

}