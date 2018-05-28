using System;
using NServiceBus;
using NServiceBus.Metrics.ServiceControl;

class Configuration
{
    void ConfigureEndpoint(BusConfiguration busConfiguration)
    {
        #region ServiceControlTTBR

        busConfiguration.SetServiceControlMetricsMessageTTBR(TimeSpan.FromHours(1));

        #endregion
    }
}
