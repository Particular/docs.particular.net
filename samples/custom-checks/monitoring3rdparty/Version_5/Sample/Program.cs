using System;
using NServiceBus;

class Program
{
    private static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.CustomChecks.Monitor3rdParty");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();

        }
    }
}