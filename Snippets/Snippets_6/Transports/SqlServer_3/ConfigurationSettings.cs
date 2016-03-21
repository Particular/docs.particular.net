namespace Snippets6.Transports.SqlServer
{
    using System;
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class ConfigurationSettings
    {

        ConfigurationSettings(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

            #endregion
        }
    }
}