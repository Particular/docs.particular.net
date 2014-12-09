using NServiceBus;

public class SqlServerConfigurationSettings
{
    void TransactionScope()
    {
        var configuration = new BusConfiguration();


        #region sqlserver-config-transactionscope

        configuration.UseTransport<SqlServerTransport>();

        #endregion
    }

    void NativeTransactions()
    {
        var configuration = new BusConfiguration();


        #region sqlserver-config-native-transactions

        configuration.UseTransport<SqlServerTransport>();
        configuration.Transactions()
            .DisableDistributedTransactions();

        #endregion
    }

    void NoTransactions()
    {
        var configuration = new BusConfiguration();

        #region sqlserver-config-no-transactions

        configuration.UseTransport<SqlServerTransport>();
        configuration.Transactions().Disable();

        #endregion
    }

    void DisableSecondaries()
    {
        var configuration = new BusConfiguration();

        #region sqlserver-config-disable-secondaries

        configuration.UseTransport<SqlServerTransport>()
            .DisableCallbackReceiver();

        #endregion
    }

    void SetSecondaryReceiverConcurrency()
    {
        var configuration = new BusConfiguration();

        #region sqlserver-config-set-secondary-concurrency

        configuration.UseTransport<SqlServerTransport>()
            .CallbackReceiverMaxConcurrency(16);

        #endregion
    }

    void Callbacks()
    {
        var configuration = new BusConfiguration();

        var bus = Bus.Create(configuration);

        #region sqlserver-config-callbacks

        var callback = bus.Send(new Request());
        callback.Register(ProcessResponse);

        #endregion

        #region sqlserver-config-callbacks-reply

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