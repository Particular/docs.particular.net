namespace Snippets6.Transports.SqlServer
{
    using System;
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class ConfigurationSettings
    {
        
        void TimeToWaitBeforeTriggeringCircuitBreaker()
        {
            #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker 3

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

            #endregion
        }
    }
}