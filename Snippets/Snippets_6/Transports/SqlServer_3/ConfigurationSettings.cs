namespace Snippets6.Transports.SqlServer
{
    using System;
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class ConfigurationSettings
    {
        
        void TimeToWaitBeforeTriggeringCircuitBreaker(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

            #endregion
        }
    }
}