using System;
using NServiceBus;

class ConfigurationSettings
{
    ConfigurationSettings(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker

        var transport = new SqlServerTransport("connectionString")
        {
            TimeToWaitBeforeTriggeringCircuitBreaker = TimeSpan.FromMinutes(3)
        };

        #endregion
    }
}