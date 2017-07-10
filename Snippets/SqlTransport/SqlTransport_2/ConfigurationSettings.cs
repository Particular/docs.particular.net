using NServiceBus;

class SqlServerConfigurationSettings
{

    void CallbackReceiverMaxConcurrency(BusConfiguration busConfiguration)
    {
        #region sqlserver-CallbackReceiverMaxConcurrency

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.CallbackReceiverMaxConcurrency(10);

        #endregion
    }

    void DisableSecondaries(BusConfiguration busConfiguration)
    {
        #region sqlserver-config-disable-secondaries

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.DisableCallbackReceiver();

        #endregion
    }
}