namespace Snippets6
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;

    public class CriticalErrorConfig
    {
        public void DefineCriticalErrorActionForAzureHost()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region DefineCriticalErrorActionForAzureHost

            // Configuring how NServicebus handles critical errors
            busConfiguration.DefineCriticalErrorAction((endpointInstance, message, exception) =>
            {
                string errorMessage = $"We got a critical exception: '{message}'\r\n{exception}";

                if (Environment.UserInteractive)
                {
                    Thread.Sleep(10000); // so that user can see on their screen the problem
                }

                string fatalMessage = $"The following critical error was encountered by NServiceBus:\n{errorMessage}\nNServiceBus is shutting down.";
                Environment.FailFast(fatalMessage, exception);
                return Task.FromResult(0);
            });

            #endregion
        }
    }
}