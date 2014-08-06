using NServiceBus;
using NServiceBus.Log4Net;

public class OverrideViaProfile
{
    #region LoggingConfigWithProfile

    public class YourProfileLoggingHandler : NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile<YourProfile>
    {
        public void Configure(IConfigureThisEndpoint specifier)
        {
            // setup your logging infrastructure then call
            Log4NetConfigurator.Configure();
        }
    }

    #endregion

    class YourProfile : IProfile
    {
    }
}