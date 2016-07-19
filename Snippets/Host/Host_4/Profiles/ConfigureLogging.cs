using NServiceBus;

#region configure_logging
class YourProfileLoggingHandler :
    IConfigureLoggingForProfile<YourProfile>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        // setup logging infrastructure
    }
}
#endregion

class YourProfile :
    IProfile
{
}