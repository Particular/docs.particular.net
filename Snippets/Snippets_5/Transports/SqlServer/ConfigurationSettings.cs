namespace Snippets5.Transports.SqlServer
{
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
            #region sqlserver-config-native-transactions 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>();
            busConfiguration.Transactions()
                .DisableDistributedTransactions();

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

        void DisableSecondaries()
        {
            #region sqlserver-config-disable-secondaries 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .DisableCallbackReceiver();

            #endregion
        }

        void Callbacks()
        {
            #region sqlserver-config-callbacks 2

            BusConfiguration busConfiguration = new BusConfiguration();
            IStartableBus bus = Bus.Create(busConfiguration);
            ICallback callback = bus.Send(new Request());
            callback.Register(ProcessResponse);
            #endregion

            #region sqlserver-config-callbacks-reply 2

            bus.Return(42);

            #endregion
        }


        void ProcessResponse(CompletionResult returnCode)
        {
        }

        private class Request : IMessage
        {
        }
    }
}