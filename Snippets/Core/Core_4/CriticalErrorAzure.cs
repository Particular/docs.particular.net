namespace Core4
{
    using System;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Logging;

    class CriticalErrorAzure
    {

        CriticalErrorAzure(Configure configure, ILog log)
        {
            #region DefineCriticalErrorActionForAzureHost

            configure.DefineCriticalErrorAction((message, exception) =>
            {
                var output = $"Critical exception: '{message}'";
                log.Error(output, exception);
                if (Environment.UserInteractive)
                {
                    // so that user can see on their screen the problem
                    Thread.Sleep(10000);
                }

                var fatalMessage = $"NServiceBus critical error:\n{message}\nShutting down.";
                Environment.FailFast(fatalMessage, exception);
            });

            #endregion
        }
    }
}