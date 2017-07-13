using NServiceBus;

#region 6to7customize_nsb_host

class CustomizingHostUpgrade :
    IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        // perform some custom configuration
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion