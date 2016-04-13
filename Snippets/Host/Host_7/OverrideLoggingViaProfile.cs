namespace Snippets6.Logging
{
    using NServiceBus;
    using NServiceBus.Logging;

    public class OverrideLoggingViaProfile
    {
        #region LoggingConfigWithProfile

        public class YourProfileLoggingHandler :
            NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile<YourProfile>
        {
            public void Configure(IConfigureThisEndpoint specifier)
            {
                // setup your logging infrastructure then call
                LogManager.Use<Log4NetFactory>();
            }
        }

        #endregion

        class YourProfile : IProfile
        {
        }

        class Log4NetFactory : LoggingFactoryDefinition
        {
            protected override ILoggerFactory GetLoggingFactory()
            {
                throw new System.NotImplementedException();
            }
        }
    }

}