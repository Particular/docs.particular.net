using NServiceBus;
using NServiceBus.Logging;


#pragma warning disable 618
#region CustomHostLogging
class CustomLogging :
    IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration configuration)
    {
        LogManager.Use<DefaultFactory>();
    }
}

#endregion
#pragma warning restore 618