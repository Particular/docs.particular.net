using NServiceBus;

using NServiceBus.Logging;

#region CustomHostLogging
class CustomLogging : IConfigureThisEndpoint
{
    public void Customize(BusConfiguration configuration)
    {
        LogManager.Use<DefaultFactory>();
    }
}

#endregion
