using System;
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

    void TimeToWaitBeforeTriggeringCircuitBreaker(BusConfiguration busConfiguration)
    {
        #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

        #endregion
    }

    void PauseAfterReceiveFailure(BusConfiguration busConfiguration)
    {
        #region sqlserver-PauseAfterReceiveFailure

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.PauseAfterReceiveFailure(TimeSpan.FromSeconds(15));

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