using System;
using NServiceBus;

public class CriticalError
{
    public void DefineCriticalErrorAction()
    {

        #region DefineCriticalErrorActionV4

        // Configuring how NServicebus handles critical errors
        Configure.With().DefineCriticalErrorAction((message, exception) =>
        {
            var output = string.Format("We got a critical exception: '{0}'\r\n{1}", message, exception);
            Console.WriteLine(output);
            // Perhaps end the process??
        });

        #endregion
    }

    public void RaiseCriticalErrorAction()
    {
        Exception theException = null;
        #region RaiseCriticalErrorV4

        // Configuring how NServicebus handles critical errors
        Configure.With().RaiseCriticalError("The message", theException);

        #endregion
    }
}