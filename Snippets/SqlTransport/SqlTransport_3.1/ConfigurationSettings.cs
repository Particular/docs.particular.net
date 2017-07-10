using System;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

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