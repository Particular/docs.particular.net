namespace Core3.Recoverability.Delayed
{
    using NServiceBus;

    class Usage
    {
        void DisableWithCode(Configure configure)
        {
            #region DisableSlrWithCode

            configure.DisableSecondLevelRetries();

            #endregion
        }
    }
}