namespace Core3.Recoverability.Delayed
{
    using NServiceBus;

    class Usage
    {
        void DisableWithCode(Configure configure)
        {
            #region DisableDelayedRetries

            configure.DisableSecondLevelRetries();

            #endregion
        }
    }
}