using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Upgrade
{

    void EnableToTrace(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11EnableToTrace

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.EnableMetricTracing(TimeSpan.FromSeconds(5));

        #endregion
    }

    void EnableToLog(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11EnableToLog

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.EnableLogTracing(TimeSpan.FromSeconds(5), LogLevel.Info);

        #endregion
    }
    void Custom(EndpointConfiguration endpointConfiguration)
    {
        #region 1to11Custom

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.EnableCustomReport(
            func: data => ProcessMetric(data),
            interval: TimeSpan.FromSeconds(5));

        #endregion
    }

    static Task ProcessMetric(string data)
    {
        return Task.CompletedTask;
    }
}