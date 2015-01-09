using log4net.Core;
using NServiceBus;

class LiteLoggingHandler : IConfigureLoggingForProfile<Lite>
{
    public void Configure(IConfigureThisEndpoint specifier)
    {
        LoggingHelper.ConfigureLogging(Level.Info);
    }
}