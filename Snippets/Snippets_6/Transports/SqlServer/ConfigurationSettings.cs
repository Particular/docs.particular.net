namespace Snippets6.Transports.SqlServer
{
    using System;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class ConfigurationSettings
    {
        public void SqlServerTransactionScopeIsolationLevel()
        {
            #region sqlserver-config-transactionscope-isolation-level 3
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<SqlServerTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void SqlServerTransactionScopeTimeout()
        {
            #region sqlserver-config-transactionscope-timeout 3
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<SqlServerTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
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