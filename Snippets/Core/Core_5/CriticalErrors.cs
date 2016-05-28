#pragma warning disable 649
namespace Core5
{
    using System;
    using System.Threading;
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

            LogManager.GetLogger("NServiceBus").Fatal(errorMessage, exception);

            #endregion
        }


        void DefaultAction(Configure configure)
        {
            //https://github.com/Particular/NServiceBus/blob/support-5.0/src/NServiceBus.Core/CriticalError/CriticalError.cs

            #region DefaultCriticalErrorAction

            var components = configure.Builder.Build<IConfigureComponents>();
            if (!components.HasComponent<IBus>())
            {
                return;
            }

            configure.Builder.Build<IStartableBus>()
                .Dispose();

            #endregion

        }

        void DefaultHostAction(string errorMessage, Exception exception)
        {
            //https://github.com/Particular/NServiceBus/blob/support-5.0/src/NServiceBus.Hosting.Windows/GenericHost.cs

            #region DefaultHostCriticalErrorAction

            if (Environment.UserInteractive)
            {
                Thread.Sleep(10000); // so that user can see on their screen the problem
            }

            var fatalMessage = $"The following critical error was encountered by NServiceBus:\n{errorMessage}\nNServiceBus is shutting down.";
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