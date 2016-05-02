using NServiceBus;

#region 6to7customize_nsb_host

class CustomizingHostUpgrade : IConfigureThisEndpoint
{
    public void Customize(BusConfiguration busConfiguration)
    {
        // perform some custom configuration
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion