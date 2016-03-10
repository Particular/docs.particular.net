using System;
using Anotar.NServiceBus;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.Anotar";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Anotar");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            LogTo.Info("Press any key to exit");
            Console.ReadKey();
        }
    }
}
