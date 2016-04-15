namespace Snippets3.Errors.SecondLevel
{
    using NServiceBus;

    class DisableWithCode
    {
        DisableWithCode(Configure configure)
        {
            #region DisableSlrWithCode

            configure.DisableSecondLevelRetries();

            #endregion
        }
    }
}