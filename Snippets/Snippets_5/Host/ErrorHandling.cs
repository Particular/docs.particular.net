namespace Snippets_5.Host
{
    using System;
    using NServiceBus;
    using NServiceBus.Logging;
    
    class ErrorHandling
    {
        public IStartableBus bus { get; set; }
        ErrorHandling()
        {
            #region CustomHostErrorHandlingSubscription

            var busConfiguration = new BusConfiguration();
            busConfiguration.DefineCriticalErrorAction(OnCriticalError);

            #endregion
        }

        #region CustomHostErrorHandlingAction

        ILog Logger = LogManager.GetLogger(typeof(ErrorHandling));
        void OnCriticalError(string errorMessage, Exception exception)
        {
            Logger.Fatal(string.Format("CRITICAL: {0}", errorMessage), exception);

            // If you want the process to be active, dispose the bus. 
            // Keep in mind that when the bus is disposed, sending messages will throw with an ObjectDisposedException.
            bus.Dispose();

            // If you want to kill the process, raise a fail fast error as shown below. 
            // Environment.FailFast(String.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage), exception);

        }
        #endregion
    }
}
