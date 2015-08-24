namespace Snippets3.Errors.SecondLevel
{
    using NServiceBus;

    public class DisableWithCode
    {
        public DisableWithCode()
        {
            #region DisableSlrWithCode

            Configure configure = Configure.With();
            configure.DisableSecondLevelRetries();

            #endregion
        }
    }
}
