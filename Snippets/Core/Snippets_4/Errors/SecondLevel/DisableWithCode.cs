namespace Core4.Errors.SecondLevel
{
    using NServiceBus;

    class DisableWithCode
    {
        DisableWithCode()
        {
            #region DisableSlrWithCode
            Configure.Features
                .Disable<NServiceBus.Features.SecondLevelRetries>();
            #endregion
        }

    }
}
