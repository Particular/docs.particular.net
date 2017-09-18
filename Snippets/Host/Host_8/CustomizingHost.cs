using NServiceBus;

#pragma warning disable 618
#region customize_nsb_host

class CustomizingHost :
    IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        // To customize, use the configuration parameter.
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion
#pragma warning restore 618