using System;
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

        #region ServiceControlTTBR

        busConfiguration.SetServiceControlMetricsMessageTTBR(TimeSpan.FromHours(1));

        #endregion
    }
}
