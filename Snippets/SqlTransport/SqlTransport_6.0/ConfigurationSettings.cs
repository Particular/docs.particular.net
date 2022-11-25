using System;
using NServiceBus;

class ConfigurationSettings
{
    ConfigurationSettings(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

        #endregion
    }
}