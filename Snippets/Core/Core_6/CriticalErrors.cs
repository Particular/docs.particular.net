#pragma warning disable 649

namespace Core6
{
    using System;
    using System.Threading;
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

            // To kill the process, await the above, then raise a fail fast error as shown below.
            //string failMessage = string.Format("Critical error shutting down:'{0}'.", context.Error);
            //Environment.FailFast(failMessage, context.Exception);
        }

        #endregion


        void DefaultActionLogging(string errorMessage, Exception exception)
        {
            #region DefaultCriticalErrorActionLogging

            LogManager.GetLogger("NServiceBus").Fatal(errorMessage, exception);

            #endregion
        }


        Task DefaultAction(IEndpointInstance endpoint)
        {
            #region DefaultCriticalErrorAction

            return endpoint.Stop();

            #endregion
        }

        void DefaultHostAction(string errorMessage, Exception exception)
        {
            //TODO: verify when host is updated to v6

            #region DefaultHostCriticalErrorAction

            if (Environment.UserInteractive)
            {
                Thread.Sleep(10000); // so that user can see on their screen the problem
            }

            string fatalMessage = $"The following critical error was encountered by NServiceBus:\n{errorMessage}\nNServiceBus is shutting down.";
            Environment.FailFast(fatalMessage, exception);

            #endregion

        }

        void InvokeCriticalError(CriticalError criticalError, string errorMessage, Exception exception)
        {
            #region InvokeCriticalError

            // 'criticalError' is an instance of the NServiceBus.CriticalError class
            // This instance can be resolved from the container.
            criticalError.Raise(errorMessage, exception);

            #endregion

        }
    }
}