using log4net.Core;
using NServiceBus;

#region ProductionHandler
class ProductionLoggingHandler :
    NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile<Production>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        LoggingHelper.ConfigureLogging(Level.Error);
    }
}
#endregion