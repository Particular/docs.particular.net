namespace Snippets5.Transports.SqlServer
{
    using System;
    using NServiceBus;

    public class SqlServerConfigurationSettings
    {
        void TransactionScope()
        {
            #region sqlserver-config-transactionscope 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>();

            #endregion
        }
        void ConnectionString()
        {
            #region sqlserver-config-connectionstring 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True");

            #endregion
        }

        void NativeTransactions()
        {
            #region sqlserver-config-native-transactions-atomicSendsReceive 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>();
            busConfiguration.Transactions()
                .DisableDistributedTransactions();

            #endregion
        }
		
        void AccessNativeTransactions()
        {
            #region sqlserver-config-native-transactions-accessTransaction 2
			/*
            class MyHandler : IHandleMessages<MyMessage>
            {
            
				//Injected property with SqlTransport storage context
                public SqlServerStorageContext StorageContext { get; set; }

                public void Handle(MyMessage message)
                {
                    var currentNativeTransaction = StorageContext.Transaction;

					...
                }
            }
            */
            #endregion
        }
        void NoTransactions()
        {
            #region sqlserver-config-no-transactions 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>();
            busConfiguration.Transactions().Disable();

            #endregion
        }
        void CallbackReceiverMaxConcurrency()
        {
            #region sqlserver-CallbackReceiverMaxConcurrency 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion
        }
        void TimeToWaitBeforeTriggeringCircuitBreaker()
        {
            #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

            #endregion
        }
        void PauseAfterReceiveFailure()
        {
            #region sqlserver-PauseAfterReceiveFailure 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .PauseAfterReceiveFailure(TimeSpan.FromSeconds(15));

            #endregion
        }

        void DisableSecondaries()
        {
            #region sqlserver-config-disable-secondaries 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .DisableCallbackReceiver();

            #endregion
        }

    }
}