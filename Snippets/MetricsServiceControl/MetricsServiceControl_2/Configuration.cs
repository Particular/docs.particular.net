using System;
using System.Net;
using System.Net.NetworkInformation;
using NServiceBus;

class Configuration
{
    void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region SendMetricDataToServiceControl
        const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

        var metrics = endpointConfiguration.EnableMetrics();

        metrics.SendMetricDataToServiceControl(
            serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
            interval: TimeSpan.FromSeconds(10),
            instanceId: "INSTANCE_ID_OPTIONAL");
        #endregion
    }
    void SendMetricDataToServiceControlHostId(EndpointConfiguration endpointConfiguration)
    {
        #region SendMetricDataToServiceControlHostId
        const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

        var endpointName = "MyEndpoint";
        var machineName = $"{Dns.GetHostName()}.{IPGlobalProperties.GetIPGlobalProperties().DomainName}";
        var instanceIdentifier = $"{endpointName}@{machineName}";

        var metrics = endpointConfiguration.EnableMetrics();

        metrics.SendMetricDataToServiceControl(
            serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
            interval: TimeSpan.FromSeconds(10),
            instanceId: instanceIdentifier);
        #endregion
    }
}
