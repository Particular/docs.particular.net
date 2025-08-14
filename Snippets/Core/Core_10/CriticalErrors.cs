namespace Core;

using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class CriticalErrors
{
    CriticalErrors(EndpointConfiguration endpointConfiguration)
    {
        #region DefiningCustomHostErrorHandlingAction

        endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

        #endregion
    }

    #region CustomHostErrorHandlingAction

    async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
    {
        try
        {
            // To leave the process active, stop the endpoint.
            // When it is stopped, attempts to send messages will cause an ObjectDisposedException.
            await context.Stop(cancellationToken);
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

    async Task CriticalErrorHandler(ICriticalErrorContext criticalErrorContext, CancellationToken cancellationToken)
    {
        #region StopEndpointInCriticalError

        await criticalErrorContext.Stop(cancellationToken);

        #endregion
    }

    void InvokeCriticalError(CriticalError criticalError, string errorMessage, Exception exception)
    {
        #region InvokeCriticalError

        // 'criticalError' is an instance of NServiceBus.CriticalError
        // This instance can be resolved from dependency injection.
        criticalError.Raise(errorMessage, exception);

        #endregion
    }
}