using NServiceBus;
using NServiceBus.Log4Net;

public class OverrideViaProfile
{
    // start code LoggingConfigWithProfile
    public class YourProfileLoggingHandler : IConfigureLoggingForProfile<YourProfile>
    {
        public void Configure(IConfigureThisEndpoint specifier)
        {
            // setup your logging infrastructure then call
            Log4NetConfigurator.Configure();
        }
    }
    // end code LoggingConfigWithProfile

    class YourProfile : IProfile
    {
    }
}
