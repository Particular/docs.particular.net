using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Versioning.V2Subscriber");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}