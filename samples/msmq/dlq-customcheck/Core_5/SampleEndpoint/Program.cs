using System;
using NServiceBus;

class Program
{
    static void Main(string[] args)
    {
        var config = new BusConfiguration();
        config.EndpointName("SampleEndpoint");
        config.UseTransport<MsmqTransport>();
        config.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(config).Start())
        {
            Console.WriteLine("Bus started");
            Console.ReadLine();
        }
    }
}