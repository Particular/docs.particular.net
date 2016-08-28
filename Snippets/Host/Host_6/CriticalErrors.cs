namespace Host_4
{
    using System;
    using System.Threading;

    class CriticalErrors
    {
        void DefaultHostAction(string errorMessage, Exception exception)
        {
            // https://github.com/Particular/NServiceBus/blob/support-5.0/src/NServiceBus.Hosting.Windows/GenericHost.cs

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
