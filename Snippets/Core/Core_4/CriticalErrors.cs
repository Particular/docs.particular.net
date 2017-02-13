#pragma warning disable 649

namespace Core4
{
    using System;
    using NServiceBus;
    using NServiceBus.Logging;

    class CriticalErrors
    {
        IStartableBus bus;

        CriticalErrors(Configure configure)
        {
            #region DefiningCustomHostErrorHandlingAction

            configure.DefineCriticalErrorAction(OnCriticalError);

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
            // https://github.com/Particular/NServiceBus/blob/support-4.0/src/NServiceBus.Core/ConfigureCriticalErrorAction.cs
            #region DefaultCriticalErrorAction

            if (!Configure.BuilderIsConfigured())
            {
                return;
            }

            if (!configure.Configurer.HasComponent<IBus>())
            {
                return;
            }

            var startableBus = configure.Builder.Build<IStartableBus>();
            startableBus.Shutdown();

            #endregion
        }

        void InvokeCriticalError(Configure configure, string errorMessage, Exception exception)
        {
            #region InvokeCriticalError

            configure.RaiseCriticalError(errorMessage, exception);

            #endregion
        }
    }
}