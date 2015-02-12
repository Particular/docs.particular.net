using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    private static void Main()
    {
        var busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.CustomChecks.Monitor3rdParty");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfig))
        {
            bus.Start();
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();

        }
    }
}