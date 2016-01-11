using NServiceBus;

#region nservicebus-host

public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server
{
    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.EndpointName("Samples.Logging.HostProfiles");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion