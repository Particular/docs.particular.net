namespace Core5
{
    using System;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Logging;

    class CriticalErrorConfig
    {
        CriticalErrorConfig(BusConfiguration busConfiguration, ILog log)
        {
            #region DefineCriticalErrorActionForAzureHost

            // Configuring how NServiceBus handles critical errors
            busConfiguration.DefineCriticalErrorAction(
                onCriticalError: (message, exception) =>
                {
                    var output = $"Critical exception: '{message}'";
                    log.Error(output, exception);
                    if (Environment.UserInteractive)
                    {
                        // so that user can see on their screen the problem
                        Thread.Sleep(10000);
                    }

                    var fatalMessage = $"Critical error:\n{message}\nShutting down.";
                    Environment.FailFast(fatalMessage, exception);
                });

            #endregion
        }
    }
}