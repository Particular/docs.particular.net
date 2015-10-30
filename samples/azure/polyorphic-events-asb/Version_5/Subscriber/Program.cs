using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.ASB.Polymorphic.Subscriber");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.ScaleOut().UseSingleBrokerQueue();
        busConfiguration.UseTransport<AzureServiceBusTransport>()
            .ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}