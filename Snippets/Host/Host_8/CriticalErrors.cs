namespace Host_4
{
    using System;
    using System.Threading;

    class CriticalErrors
    {
        void DefaultHostAction(string errorMessage, Exception exception)
        {
            //TODO: verify when host is updated to v6

            #region DefaultHostCriticalErrorAction

            if (Environment.UserInteractive)
            {
                // so that user can see on their screen the problem
                Thread.Sleep(10000);
            }

            var fatalMessage = $"NServiceBus critical error:\n{errorMessage}\nShutting down.";
            Environment.FailFast(fatalMessage, exception);

            #endregion

        }
    }
}
