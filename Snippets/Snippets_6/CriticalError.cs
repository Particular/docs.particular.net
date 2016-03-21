namespace Snippets6
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class CriticalErrorConfig
    {
        CriticalErrorConfig(EndpointConfiguration endpointConfiguration, ILog log)
        {
            #region DefineCriticalErrorActionForAzureHost

            // Configuring how NServicebus handles critical errors
            endpointConfiguration.DefineCriticalErrorAction(context =>
            {
                string output = $"Critical exception: '{context.Error}'";
                log.Error(output, context.Exception);
                if (Environment.UserInteractive)
                {
                    Thread.Sleep(10000); // so that user can see on their screen the problem
                }

                string fatalMessage = $"The following critical error was encountered by NServiceBus:\n{context.Error}\nNServiceBus is shutting down.";
                Environment.FailFast(fatalMessage, context.Exception);
                return Task.FromResult(0);
            });

            #endregion
        }
    }
}