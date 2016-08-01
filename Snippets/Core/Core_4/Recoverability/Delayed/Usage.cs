namespace Core4.Recoverability.Delayed
{
    using NServiceBus;

    class Usage
    {
        void DisableWithCode()
        {
            #region DisableSlrWithCode
            Configure.Features
                .Disable<NServiceBus.Features.SecondLevelRetries>();
            #endregion
        }

    }
}
