#pragma warning disable 649
namespace Snippets4.Host
{
    using System;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Logging;

    public static class BusInstance
    {
        public static IBus Bus { get; private set; }

        public static void SetInstance(IBus bus)
        {
            Bus = bus;
        }
    } 
    class CriticalErrors
    {
        IStartableBus bus;

        CriticalErrors()
        {
            #region DefiningCustomHostErrorHandlingAction

            Configure.Instance.DefineCriticalErrorAction(OnCriticalError);

            #endregion
        }

        #region CustomHostErrorHandlingAction

        void OnCriticalError(string errorMessage, Exception exception)
        {
            // If you want the process to be active, dispose the bus. 
            // Note that when the bus is disposed sending messages will throw with an ObjectDisposedException.
            bus.Dispose();

            // If you want to kill the process, raise a fail fast error as shown below. 
            //string failMessage = string.Format("Critical error shutting down:'{0}'.", errorMessage);
            //Environment.FailFast(failMessage, exception);
        }

        #endregion


        void DefaultActionLogging()
        {

            string errorMessage = null;
            Exception exception = null;
            #region DefaultCriticalErrorActionLogging

            LogManager.GetLogger("NServiceBus").Fatal(errorMessage, exception);

            #endregion
        }
        void DefaultAction()
        {

            //https://github.com/Particular/NServiceBus/blob/support-4.0/src/NServiceBus.Core/ConfigureCriticalErrorAction.cs
            #region DefaultCriticalErrorAction 

            if (!Configure.BuilderIsConfigured())
                return;

            if (!Configure.Instance.Configurer.HasComponent<IBus>())
                return;

            Configure.Instance.Builder.Build<IStartableBus>()
                .Shutdown();

            #endregion

        }
        void DefaultHostAction()
        {

            string errorMessage = null;
            Exception exception = null;

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

        void InvokeCriticalError()
        {

            string errorMessage = null;
            Exception exception = null;
            #region InvokeCriticalError

            Configure.Instance.RaiseCriticalError(errorMessage, exception);

            #endregion

        }
    }
}