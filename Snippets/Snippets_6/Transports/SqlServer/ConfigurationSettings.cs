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
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void SqlServerTransactionScopeTimeout()
        {
            #region sqlserver-config-transactionscope-timeout 3
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}