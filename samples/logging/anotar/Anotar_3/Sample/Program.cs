using System;
using Anotar.NServiceBus;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.Anotar";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Anotar");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            LogTo.Info("Press any key to exit");
            Console.ReadKey();
        }
    }
}
