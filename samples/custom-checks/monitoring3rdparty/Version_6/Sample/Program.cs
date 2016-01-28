using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomChecks.Monitor3rdParty");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        Endpoint.Start(busConfiguration).GetAwaiter().GetResult();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}