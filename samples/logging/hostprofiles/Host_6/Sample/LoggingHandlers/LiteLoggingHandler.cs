using log4net.Core;
using NServiceBus;

#region LiteHandler
class LiteLoggingHandler :
    NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile<Lite>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        LoggingHelper.ConfigureLogging(Level.Info);
    }
}
#endregion