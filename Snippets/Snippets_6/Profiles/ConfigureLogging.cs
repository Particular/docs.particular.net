using NServiceBus;

#region configure_logging
class YourProfileLoggingHandler : NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile<YourProfile>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        // setup your logging infrastructure here
    }
}
#endregion

internal class YourProfile : IProfile
{
}