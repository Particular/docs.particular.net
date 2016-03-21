namespace Snippets4
{
    using System;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Logging;

    class CriticalError
    {

        CriticalError(Configure configure, ILog log)
        {
            #region DefineCriticalErrorActionForAzureHost

            configure.DefineCriticalErrorAction((message, exception) =>
            {
                string output = string.Format("Critical exception: '{0}'", message);
                log.Error(output, exception);
                if (Environment.UserInteractive)
                {
                    Thread.Sleep(10000); // so that user can see on their screen the problem
                }

                string fatalMessage = string.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", message);
                Environment.FailFast(fatalMessage, exception);
            });

            #endregion
        }
    }
}