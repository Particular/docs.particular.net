using NServiceBus;

public class ServiceBus
{
    public static IBus Bus { get; private set; }

    static readonly object padlock = new object();

    public static void Init()
    {
        if (Bus != null)
            return;

        lock (padlock)
        {
            if (Bus != null)
                return;

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.Callbacks.WebSender");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();
            busConfiguration.SendFailedMessagesTo("error");

            Bus = NServiceBus.Bus.Create(busConfiguration).Start();
        }
    }
}
