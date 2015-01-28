using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("FullDuplexSample_Server");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Console.WriteLine("To exit, Ctrl + C");
            Console.ReadLine();
        }
    }
}