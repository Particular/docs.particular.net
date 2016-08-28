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

        Task OnCriticalError(ICriticalErrorContext context)
        {
            // To leave the process active, stop the endpoint.
            return context.Stop();

            // To kill the process, await the above,
            // then raise a fail fast error as shown below.
            // var failMessage = $"Critical error shutting down:'{context.Error}'.";
            // Environment.FailFast(failMessage, context.Exception);
        }

        #endregion


        void DefaultActionLogging(string errorMessage, Exception exception)
        {
            #region DefaultCriticalErrorActionLogging

            var logger = LogManager.GetLogger("NServiceBus");
            logger.Fatal(errorMessage, exception);

            #endregion
        }


        Task DefaultAction(IEndpointInstance endpoint)
        {
            #region DefaultCriticalErrorAction

            return endpoint.Stop();

            #endregion
        }


        void InvokeCriticalError(CriticalError criticalError, string errorMessage, Exception exception)
        {
            #region InvokeCriticalError

            // 'criticalError' is an instance of NServiceBus.CriticalError
            // This instance can be resolved from the container.
            criticalError.Raise(errorMessage, exception);

            #endregion
        }
    }
}