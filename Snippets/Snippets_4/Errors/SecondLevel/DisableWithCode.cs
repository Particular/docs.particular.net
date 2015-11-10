namespace Snippets4.Errors.SecondLevel
{
    using NServiceBus;

    public class DisableWithCode
    {
        public DisableWithCode()
        {
            #region DisableSlrWithCode
            Configure.Features
                .Disable<NServiceBus.Features.SecondLevelRetries>();
            #endregion
        }

    }
}
