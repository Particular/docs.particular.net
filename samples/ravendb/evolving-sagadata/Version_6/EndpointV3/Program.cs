using System;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client.Document;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.RavenDB.Migration");
        DocumentStore documentStore = new DocumentStore
        {
            Url = "http://localhost:8083",
            DefaultDatabase = "RavenSampleData",
        };
        documentStore.Initialize();

        busConfiguration.UsePersistence<RavenDBPersistence>()
            .DoNotSetupDatabasePermissions() //Only required to simplify the sample setup
            .SetDefaultDocumentStore(documentStore);

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Console.WriteLine("Press 'S' to start reset the saga");
            Console.WriteLine("Press 'I' to increment the order count");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.S)
                {
                    await endpoint.SendLocal(new StartOrder
                    {
                        OrderId = 10,
                        ItemCount = 5
                    });
                    continue;
                }

                if (key.Key == ConsoleKey.I)
                {
                    await endpoint.SendLocal(new IncrementOrder
                    {
                        OrderId = 10,
                    });
                    continue;
                }
                return;
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}