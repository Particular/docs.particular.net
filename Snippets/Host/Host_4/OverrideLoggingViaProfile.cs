using NServiceBus;

class OverrideLoggingViaProfile
{
    #region LoggingConfigWithProfile

    public class YourProfileLoggingHandler :
        IConfigureLoggingForProfile<YourProfile>
    {
        public void Configure(IConfigureThisEndpoint specifier)
        {
            // setup logging infrastructure
            SetLoggingLibrary.Log4Net();
        }
    }

    #endregion

    class YourProfile :
        IProfile
    {
    }
}