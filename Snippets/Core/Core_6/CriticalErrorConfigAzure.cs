namespace Core6
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class CriticalErrorConfigAzure
    {
        CriticalErrorConfigAzure(EndpointConfiguration endpointConfiguration, ILog log)
        {
            #region DefineCriticalErrorActionForAzureHost

            // Configuring how NServiceBus handles critical errors
            endpointConfiguration.DefineCriticalErrorAction(context =>
            {
                string output = $"Critical exception: '{context.Error}'";
                log.Error(output, context.Exception);
                if (Environment.UserInteractive)
                {
                    // so that user can see on their screen the problem
                    Thread.Sleep(10000);
                }

                string fatalMessage = $"NServiceBus critical error:\n{context.Error}\nShutting down.";
                Environment.FailFast(fatalMessage, context.Exception);
                return Task.FromResult(0);
            });

            #endregion
        }
    }
}