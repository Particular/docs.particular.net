#pragma warning disable 649
namespace Snippets3.Host
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using log4net;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;

    class CriticalErrors
    {

        IBus bus;

        CriticalErrors()
        {
            #region DefiningCustomHostErrorHandlingAction

            Configure.Instance.DefineCriticalErrorAction(OnCriticalError);

            #endregion
        }

        #region CustomHostErrorHandlingAction

        static ILog logger = LogManager.GetLogger(typeof(CriticalErrors));

        void OnCriticalError()
        {
            //Write log entry in version 3 since this is not done by default.
            logger.Fatal("CRITICAL Error");

            // If you want the process to be active, dispose the bus. 
            // Note that when the bus is disposed sending messages will throw with an ObjectDisposedException.
            ((IDisposable)bus).Dispose();

            // If you want to kill the process, raise a fail fast error as shown below. 
            //string failMessage = string.Format("Critical error shutting down:'{0}'.", errorMessage);
            //Environment.FailFast("Fatal bus exception");
        }

        #endregion


        void DefaultAction()
        {

            //https://github.com/Particular/NServiceBus/blob/support-3.3/src/faults/NServiceBus.Faults/Configuration/ConfigureCriticalErrorAction.cs

            #region DefaultCriticalErrorAction

            Configure.Instance.Builder.Build<ITransport>()
                .ChangeNumberOfWorkerThreads(0);

            #endregion
        }

        void DefaultHostAction()
        {

            //https://github.com/Particular/NServiceBus/blob/support-3.3/src/hosting/NServiceBus.Hosting.Windows/WindowsHost.cs

            #region DefaultHostCriticalErrorAction

            Thread.Sleep(10000); // so that user can see on their screen the problem
            Process.GetCurrentProcess().Kill();

            #endregion

        }

        void InvokeCriticalError()
        {

            #region InvokeCriticalError

            Configure.Instance.OnCriticalError();

            #endregion

        }

    }
}