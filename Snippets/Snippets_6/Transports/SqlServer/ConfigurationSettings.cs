namespace Snippets6.Transports.SqlServer
{
    using System;
    using NServiceBus;

    public class SqlServerConfigurationSettingsV3
    { void TransactionScope()
        {
            #region sqlserver-config-transactionscope 3

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>();

            #endregion
        }
        void ConnectionString()
        {
            #region sqlserver-config-connectionstring 3

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True");

            #endregion
        }
		
        void NativeTransactionsSendsAtomicWithReceive()
        {
            #region sqlserver-config-native-transactions-atomicSendsReceive 3
			BusConfiguration busConfiguration = new BusConfiguration();
			busConfiguration.UseTransport<SqlServerTransport>()
				.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

            #endregion
        }
		
        void NativeTransactionsReceiveOnly()
        {
            #region sqlserver-config-native-transactions-receiveOnly 3
			BusConfiguration busConfiguration = new BusConfiguration();
			busConfiguration.UseTransport<SqlServerTransport>()
				.Transactions(TransportTransactionMode.ReceiveOnly);

            #endregion
        }

        void NoTransactions()
        {
            #region sqlserver-config-no-transactions 3

            BusConfiguration busConfiguration = new BusConfiguration();
			busConfiguration.UseTransport<SqlServerTransport>()
				.Transactions(TransportTransactionMode.None);

            #endregion
        }
		
		void TimeToWaitBeforeTriggeringCircuitBreaker()
        {
            #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker 3

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

            #endregion
        }
    }
}