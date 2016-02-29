using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Scaleout.Worker2";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Worker2");
        busConfiguration.EnlistWithMSMQDistributor();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}