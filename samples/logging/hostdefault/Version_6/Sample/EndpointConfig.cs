using System.IO;
using NServiceBus;
using NServiceBus.Logging;

#region nservicebus-host

public class EndpointConfig : IConfigureThisEndpoint
{
    public EndpointConfig()
    {
        DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Directory(Path.GetTempPath());
        defaultFactory.Level(LogLevel.Debug);
    }

    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.EndpointName("Samples.Logging.HostDefault");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion