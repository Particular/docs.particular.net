namespace Core6
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

            // Configuring how NServiceBus handles critical errors
            endpointConfiguration.DefineCriticalErrorAction(
                onCriticalError: context =>
                {
                    var output = $"Critical exception: '{context.Error}'";
                    log.Error(output, context.Exception);
                    if (Environment.UserInteractive)
                    {
                        // so that user can see on their screen the problem
                        Thread.Sleep(10000);
                    }

                    var fatalMessage = $"Critical error:\n{context.Error}\nShutting down.";
                    Environment.FailFast(fatalMessage, context.Exception);
                    return Task.CompletedTask;
                });

            #endregion
        }
    }
}