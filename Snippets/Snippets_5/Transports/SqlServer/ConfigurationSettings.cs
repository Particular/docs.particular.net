namespace Snippets5.Transports.SqlServer
{
    using System;
    using NServiceBus;

    public class SqlServerConfigurationSettings
    {
        

        void CallbackReceiverMaxConcurrency()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-CallbackReceiverMaxConcurrency 2

            busConfiguration.UseTransport<SqlServerTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion
        }
        void TimeToWaitBeforeTriggeringCircuitBreaker()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker 2

            busConfiguration.UseTransport<SqlServerTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

            #endregion
        }
        void PauseAfterReceiveFailure()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-PauseAfterReceiveFailure 2

            busConfiguration.UseTransport<SqlServerTransport>()
                .PauseAfterReceiveFailure(TimeSpan.FromSeconds(15));

            #endregion
        }

        void DisableSecondaries()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-config-disable-secondaries 2

            busConfiguration.UseTransport<SqlServerTransport>()
                .DisableCallbackReceiver();

            #endregion
        }

    }
}