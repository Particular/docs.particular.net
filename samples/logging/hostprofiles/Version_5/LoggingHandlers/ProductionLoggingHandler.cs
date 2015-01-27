using log4net.Core;
using NServiceBus;

class ProductionLoggingHandler : NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile<Production>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        LoggingHelper.ConfigureLogging(Level.Error);
    }
}