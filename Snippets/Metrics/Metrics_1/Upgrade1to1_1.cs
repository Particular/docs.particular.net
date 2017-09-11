using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Upgrade1to1_1
{

    void EnableToTrace(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11EnableToTrace

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.EnableMetricTracing(TimeSpan.FromSeconds(5));

        #endregion
    }

    void EnableToLog(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11EnableToLog

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.EnableLogTracing(TimeSpan.FromSeconds(5), LogLevel.Info);

        #endregion
    }
    void Custom(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11Custom

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.EnableCustomReport(
            func: data => ProcessMetric(data),
            interval: TimeSpan.FromSeconds(5));

        #endregion
    }

    static Task ProcessMetric(string data)
    {
        return Task.CompletedTask;
    }
}