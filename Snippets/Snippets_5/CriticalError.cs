using System;
using System.Threading;
using NServiceBus;

public class CriticalErrorConfig
{
    public void DefineCriticalErrorAction()
    {

        #region DefineCriticalErrorAction

        BusConfiguration busConfiguration = new BusConfiguration();

        // Configuring how NServicebus handles critical errors
        busConfiguration.DefineCriticalErrorAction((message, exception) =>
        {
            string output = string.Format("We got a critical exception: '{0}'\r\n{1}", message, exception);
            Console.WriteLine(output);
            // Perhaps end the process??
        });

        #endregion
    }

    #region RaiseCriticalError

    //This could be a handler, a saga or some other service injected into the container
    public class MyService
    {
        CriticalError criticalError;

        // the CriticalError instance will be injected at runtime
        public MyService(CriticalError criticalError)
        {
            this.criticalError = criticalError;
        }

        // this would be called in some case where you want the CriticalErrorAction executed
        public void RaiseCriticalErrorAction(Exception theException)
        {
            criticalError.Raise("The message", theException);
        }
    }

    #endregion

    public void DefineCriticalErrorActionForAzureHost()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        #region DefineCriticalErrorActionForAzureHost

        // Configuring how NServicebus handles critical errors
        busConfiguration.DefineCriticalErrorAction((message, exception) =>
        {
            string errorMessage = string.Format("We got a critical exception: '{0}'\r\n{1}", message, exception);

            if (Environment.UserInteractive)
            {
                Thread.Sleep(10000); // so that user can see on their screen the problem
            }

            Environment.FailFast(string.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage), exception);
        });

        #endregion
    }
}