using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Azure.ServiceBus.Endpoint2";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Azure.ServiceBus.Endpoint2");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.ScaleOut().UseSingleBrokerQueue();
        busConfiguration.UseTransport<AzureServiceBusTransport>()
            .ConnectionString(Environment.GetEnvironmentVariable("SamplesAzureServiceBusConnection"));
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}