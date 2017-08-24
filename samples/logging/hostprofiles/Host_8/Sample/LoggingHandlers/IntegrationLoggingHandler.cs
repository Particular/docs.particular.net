using log4net.Core;
using NServiceBus;
#region IntegrationHandler
class IntegrationLoggingHandler :
    NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile<Integration>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        LoggingHelper.ConfigureLogging(Level.Warn);
    }
}
#endregion