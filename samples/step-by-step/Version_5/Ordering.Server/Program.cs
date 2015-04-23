using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("StepByStep.Ordering.Server");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();
            Console.WriteLine("To exit press 'Ctrl + C'");
            Console.ReadLine();
        }
    }
}
