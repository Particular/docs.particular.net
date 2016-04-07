using NServiceBus;

#region AzureSharedHosting_HostConfiguration

public class EndpointConfig : IConfigureThisEndpoint, AsA_Host
{
    public void Customize(BusConfiguration busConfiguration)
    {
    }
}

#endregion
