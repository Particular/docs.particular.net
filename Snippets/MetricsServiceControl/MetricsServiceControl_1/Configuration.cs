using System;
using System.Net;
using System.Net.NetworkInformation;
using NServiceBus;
using NServiceBus.Metrics.ServiceControl;

class Configuration
{
    void ConfigureEndpoint(BusConfiguration busConfiguration)
    {
        #region SendMetricDataToServiceControl

        const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

        busConfiguration.SendMetricDataToServiceControl(
            serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
            instanceId: "INSTANCE_ID_OPTIONAL");

        #endregion
    }
    void SendMetricDataToServiceControlHostId(BusConfiguration busConfiguration)
    {
        #region SendMetricDataToServiceControlHostId
        const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

        var endpointName = "MyEndpoint";
        var machineName = $"{Dns.GetHostName()}.{IPGlobalProperties.GetIPGlobalProperties().DomainName}";
        var instanceIdentifier = $"{endpointName}@{machineName}";

        busConfiguration.SendMetricDataToServiceControl(
            serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
            instanceId: instanceIdentifier);
        #endregion
    }
}
