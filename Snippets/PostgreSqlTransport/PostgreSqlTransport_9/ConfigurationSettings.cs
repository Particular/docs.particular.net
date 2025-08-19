using System;
using NServiceBus;

class ConfigurationSettings
{
    ConfigurationSettings(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-TimeToWaitBeforeTriggeringCircuitBreaker

        var transport = new PostgreSqlTransport("connectionString")
        {
            TimeToWaitBeforeTriggeringCircuitBreaker = TimeSpan.FromMinutes(3)
        };

        #endregion
    }
}