#pragma warning disable 649

namespace Core5
{
    using System;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.ObjectBuilder;

    class CriticalErrors
    {
        IStartableBus bus;

        CriticalErrors(BusConfiguration busConfiguration)
        {
            #region DefiningCustomHostErrorHandlingAction

            busConfiguration.DefineCriticalErrorAction(OnCriticalError);

            #endregion
        }

        #region CustomHostErrorHandlingAction

        void OnCriticalError(string errorMessage, Exception exception)
        {
            try
            {
                // To leave the process active, dispose the bus.
                // When the bus is disposed, the attempt to send message will cause an ObjectDisposedException.
                bus.Dispose();
                // Perform custom actions here, e.g.
                // NLog.LogManager.Shutdown();
            }
            finally
            {
                var failMessage = $"Critical error shutting down:'{errorMessage}'.";
                Environment.FailFast(failMessage, exception);
            }
        }

        #endregion


        void DefaultActionLogging(string errorMessage, Exception exception)
        {
            #region DefaultCriticalErrorActionLogging

            var logger = LogManager.GetLogger("NServiceBus");
            logger.Fatal(errorMessage, exception);

            #endregion
        }


        void DefaultAction(Configure configure)
        {
            // https://github.com/Particular/NServiceBus/blob/support-5.0/src/NServiceBus.Core/CriticalError/CriticalError.cs

            #region DefaultCriticalErrorAction

            var builder = configure.Builder;
            var components = builder.Build<IConfigureComponents>();
            if (!components.HasComponent<IBus>())
            {
                return;
            }

            var startableBus = builder.Build<IStartableBus>();
            startableBus.Dispose();

            #endregion
        }

        void InvokeCriticalError(CriticalError criticalError, string errorMessage, Exception exception)
        {
            #region InvokeCriticalError

            // 'criticalError' is an instance of NServiceBus.CriticalError
            // This instance can be resolved from dependency injection
            criticalError.Raise(errorMessage, exception);

            #endregion
        }
    }
}