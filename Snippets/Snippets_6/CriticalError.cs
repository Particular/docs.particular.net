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
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region DefineCriticalErrorActionForAzureHost

            // Configuring how NServicebus handles critical errors
            endpointConfiguration.DefineCriticalErrorAction(context =>
            {
                string errorMessage = $"We got a critical exception: '{context.Error}'\r\n{context.Exception}";

                if (Environment.UserInteractive)
                {
                    Thread.Sleep(10000); // so that user can see on their screen the problem
                }

                string fatalMessage = $"The following critical error was encountered by NServiceBus:\n{errorMessage}\nNServiceBus is shutting down.";
                Environment.FailFast(fatalMessage, context.Exception);
                return Task.FromResult(0);
            });

            #endregion
        }
    }
}