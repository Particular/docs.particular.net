using NServiceBus;
using NServiceBus.Logging;

#region CustomHostLogging
class CustomLogging : IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration configuration)
    {
        LogManager.Use<DefaultFactory>();
    }
}

#endregion