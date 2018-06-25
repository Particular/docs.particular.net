#pragma warning disable 649

namespace Core6
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class CriticalErrors
    {
        CriticalErrors(EndpointConfiguration endpointConfiguration)
        {
            #region DefiningCustomHostErrorHandlingAction

            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

            #endregion
        }

        #region CustomHostErrorHandlingAction

        async Task OnCriticalError(ICriticalErrorContext context)
        {
            try
            {
                // To leave the process active, dispose the bus.
                // When the bus is disposed, the attempt to send message will cause an ObjectDisposedException.
                await context.Stop().ConfigureAwait(false);
                // Perform custom actions here, e.g.
                // NLog.LogManager.Shutdown();
            }
            finally
            {
                var failMessage = $"Critical error shutting down:'{context.Error}'.";
                Environment.FailFast(failMessage, context.Exception);
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