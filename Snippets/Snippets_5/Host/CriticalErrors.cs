#pragma warning disable 649
using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.ObjectBuilder;

class CriticalErrors
{
    IStartableBus bus;

    CriticalErrors()
    {
        #region DefiningCustomHostErrorHandlingAction

        var busConfiguration = new BusConfiguration();
        busConfiguration.DefineCriticalErrorAction(OnCriticalError);

        #endregion
    }

    #region CustomHostErrorHandlingAction

    void OnCriticalError(string errorMessage, Exception exception)
    {
        // If you want the process to be active, dispose the bus. 
        // Keep in mind that when the bus is disposed, sending messages will throw with an ObjectDisposedException.
        bus.Dispose();

        // If you want to kill the process, raise a fail fast error as shown below. 
        // Environment.FailFast(String.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage), exception);
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

        Configure configure = null;
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

    void DefaultHostAction()
    {

        string errorMessage = null;
        Exception exception = null;

        //https://github.com/Particular/NServiceBus/blob/support-5.0/src/NServiceBus.Hosting.Windows/GenericHost.cs

        #region DefaultHostCriticalErrorAction

        if (Environment.UserInteractive)
        {
            Thread.Sleep(10000); // so that user can see on their screen the problem
        }

        Environment.FailFast(string.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage), exception);

        #endregion

    }
}