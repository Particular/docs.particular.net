using System;

class Recoverability
{

    #region Recoverability-pseudo-code

    static void DelayedRetries()
    {
        Exception exception = null;
        for (var i = 0; i <= MaxNumberOfRetries; i++)
        {
            try
            {
                ImmediateRetries();
                exception = null;
                break;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        if (exception != null)
        {
            MoveToError();
        }
    }

    static void ImmediateRetries()
    {
        Exception exception = null;
        for (var i = 0; i <= MaxNumberOfRetries; i++)
        {
            try
            {
                InvokeMessageHandling();
                exception = null;
                break;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        if (exception != null)
        {
            throw exception;
        }
    }


    #endregion

    static void InvokeMessageHandling()
    {
    }

    static void MoveToError()
    {
    }

    static int MaxNumberOfRetries = 10;
}