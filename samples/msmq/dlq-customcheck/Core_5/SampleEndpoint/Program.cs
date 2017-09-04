using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("SampleEndpoint");
        busConfiguration.UseTransport<MsmqTransport>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Bus started");
            Console.ReadLine();
        }
    }
}