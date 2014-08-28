using System;
using System.Diagnostics;
using NServiceBus;


public class CriticalErrorConfig
{
    public void DefineCriticalErrorAction()
    {

        #region DefineCriticalErrorActionV5

        var configuration = new BusConfiguration();

        // Configuring how NServicebus handles critical errors
        configuration.DefineCriticalErrorAction((message, exception) =>
        {
            var output = string.Format("We got a critical exception: '{0}'\r\n{1}", message, exception);
            Debug.WriteLine(output);
            // Perhaps end the process??
        });

        #endregion
    }

    #region RaiseCriticalErrorV5

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
}