namespace Snippets5
{
    using System;
    using System.Threading;
    using NServiceBus;

    public class CriticalErrorConfig
    {
        CriticalErrorConfig(BusConfiguration busConfiguration)
        {
            #region DefineCriticalErrorActionForAzureHost

            // Configuring how NServicebus handles critical errors
            busConfiguration.DefineCriticalErrorAction((message, exception) =>
            {
                string errorMessage = string.Format("We got a critical exception: '{0}'\r\n{1}", message, exception);

                if (Environment.UserInteractive)
                {
                    Thread.Sleep(10000); // so that user can see on their screen the problem
                }

                string fatalMessage = string.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage);
                Environment.FailFast(fatalMessage, exception);
            });

            #endregion
        }
    }
}