#pragma warning disable 649
namespace Core3.Host
{
    using System;
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

        static ILog log = LogManager.GetLogger(typeof(CriticalErrors));

        void OnCriticalError()
        {
            // Write log entry in version 3 since this is not done by default.
            log.Fatal("CRITICAL Error");

            // To leave the process active, dispose the bus.
            // When the bus is disposed sending messages will throw an ObjectDisposedException.
            ((IDisposable)bus).Dispose();

            // To kill the process, raise a fail fast error as shown below.
            // var failMessage = "Critical error shutting.";
            // Environment.FailFast(failMessage);
        }

        #endregion


        void DefaultAction()
        {
            // https://github.com/Particular/NServiceBus/blob/support-3.3/src/faults/NServiceBus.Faults/Configuration/ConfigureCriticalErrorAction.cs

            #region DefaultCriticalErrorAction

            var transport = Configure.Instance.Builder.Build<ITransport>();
            transport.ChangeNumberOfWorkerThreads(0);

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