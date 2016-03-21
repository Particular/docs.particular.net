namespace Snippets5.Logging
{
    using NServiceBus;
    using NServiceBus.Log4Net;

    class OverrideViaProfile
    {
        #region LoggingConfigWithProfile

        public class YourProfileLoggingHandler : 
            NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile<YourProfile>
        {
            public void Configure(IConfigureThisEndpoint specifier)
            {
                // setup your logging infrastructure then call
                NServiceBus.Logging.LogManager.Use<Log4NetFactory>();
            }
        }

        #endregion

        class YourProfile : IProfile
        {
        }
    }
}