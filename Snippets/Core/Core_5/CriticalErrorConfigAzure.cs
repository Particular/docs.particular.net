namespace Core5
{
    using System;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Logging;

    class CriticalErrorConfigAzure
    {
        CriticalErrorConfigAzure(BusConfiguration busConfiguration, ILog log)
        {
            #region DefineCriticalErrorActionForAzureHost

            // Configuring how NServicebus handles critical errors
            busConfiguration.DefineCriticalErrorAction((message, exception) =>
            {
                var output = $"Critical exception: '{message}'";
                log.Error(output, exception);
                if (Environment.UserInteractive)
                {
                    Thread.Sleep(10000); // so that user can see on their screen the problem
                }

                var fatalMessage = $"The following critical error was encountered by NServiceBus:\n{message}\nNServiceBus is shutting down.";
                Environment.FailFast(fatalMessage, exception);
            });

            #endregion
        }
    }
}