#pragma warning disable 649

namespace Core4
{
    using System;
    using System.Threading;
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
            // To leave the process active, dispose the bus.
            // Note that when the bus is disposed sending messages will throw with an ObjectDisposedException.
            bus.Dispose();

            // To kill the process, raise a fail fast error as shown below.
            //string failMessage = string.Format("Critical error shutting down:'{0}'.", errorMessage);
            //Environment.FailFast(failMessage, exception);
        }

        #endregion


        void DefaultActionLogging(string errorMessage, Exception exception)
        {
            #region DefaultCriticalErrorActionLogging

            LogManager.GetLogger("NServiceBus")
                .Fatal(errorMessage, exception);

            #endregion
        }

        void DefaultAction(Configure configure)
        {
            //https://github.com/Particular/NServiceBus/blob/support-4.0/src/NServiceBus.Core/ConfigureCriticalErrorAction.cs
            #region DefaultCriticalErrorAction

            if (!Configure.BuilderIsConfigured())
            {
                return;
            }

            if (!configure.Configurer.HasComponent<IBus>())
            {
                return;
            }

            configure.Builder.Build<IStartableBus>()
                .Shutdown();

            #endregion
        }

        void DefaultHostAction(string errorMessage, Exception exception)
        {
            //https://github.com/Particular/NServiceBus/blob/support-4.7/src/NServiceBus.Hosting.Windows/WindowsHost.cs

            #region DefaultHostCriticalErrorAction

            if (Environment.UserInteractive)
            {
                Thread.Sleep(10000); // so that user can see on their screen the problem
            }

            string fatalMessage = string.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage);
            Environment.FailFast(fatalMessage, exception);

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