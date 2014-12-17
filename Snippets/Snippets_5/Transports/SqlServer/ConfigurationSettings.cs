using NServiceBus;

public class SqlServerConfigurationSettings
{
    void TransactionScope()
    {
        #region sqlserver-config-transactionscope 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<SqlServerTransport>();

        #endregion
    }

    void NativeTransactions()
    {
        #region sqlserver-config-native-transactions 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<SqlServerTransport>();
        configuration.Transactions()
            .DisableDistributedTransactions();

        #endregion
    }

    void NoTransactions()
    {
        #region sqlserver-config-no-transactions 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<SqlServerTransport>();
        configuration.Transactions().Disable();

        #endregion
    }

    void DisableSecondaries()
    {
        #region sqlserver-config-disable-secondaries 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<SqlServerTransport>()
            .DisableCallbackReceiver();

        #endregion
    }

    void SetSecondaryReceiverConcurrency()
    {

        #region sqlserver-config-set-secondary-concurrency 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<SqlServerTransport>()
            .CallbackReceiverMaxConcurrency(16);

        #endregion
    }

    void Callbacks()
    {
        #region sqlserver-config-callbacks 2

        var configuration = new BusConfiguration();
        var bus = Bus.Create(configuration);
        var callback = bus.Send(new Request());
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