namespace Snippets4
{
    using System;
    using System.Threading;
    using NServiceBus;

    class CriticalError
    {

        CriticalError(Configure configure)
        {
            #region DefineCriticalErrorActionForAzureHost

            configure.DefineCriticalErrorAction((message, exception) =>
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