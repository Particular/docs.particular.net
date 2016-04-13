namespace SqlServer_2
{
    using System;
    using NServiceBus;

    class SqlServerConfigurationSettings
    {

        void CallbackReceiverMaxConcurrency(BusConfiguration busConfiguration)
        {
            #region sqlserver-CallbackReceiverMaxConcurrency

            busConfiguration.UseTransport<SqlServerTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion
        }

        void TimeToWaitBeforeTriggeringCircuitBreaker(BusConfiguration busConfiguration)
        {
            #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker

            busConfiguration.UseTransport<SqlServerTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

            #endregion
        }

        void PauseAfterReceiveFailure(BusConfiguration busConfiguration)
        {
            #region sqlserver-PauseAfterReceiveFailure

            busConfiguration.UseTransport<SqlServerTransport>()
                .PauseAfterReceiveFailure(TimeSpan.FromSeconds(15));

            #endregion
        }

        void DisableSecondaries(BusConfiguration busConfiguration)
        {
            #region sqlserver-config-disable-secondaries

            busConfiguration.UseTransport<SqlServerTransport>()
                .DisableCallbackReceiver();

            #endregion
        }

    }
}