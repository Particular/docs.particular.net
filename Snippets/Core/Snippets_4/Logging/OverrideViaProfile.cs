namespace Core4.Logging
{
    using NServiceBus;

    class OverrideViaProfile
    {
        #region LoggingConfigWithProfile

        public class YourProfileLoggingHandler : 
            IConfigureLoggingForProfile<YourProfile>
        {
            public void Configure(IConfigureThisEndpoint specifier)
            {
                // setup your logging infrastructure then call
                SetLoggingLibrary.Log4Net();
            }
        }

        #endregion

        class YourProfile : IProfile
        {
        }
    }
}