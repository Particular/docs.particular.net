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
        Console.Title = "Samples.RavenDB.Migration";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.RavenDB.Migration");
        DocumentStore documentStore = new DocumentStore
        {
            Url = "http://localhost:8083",
            DefaultDatabase = "RavenSampleData",
        };
        documentStore.Initialize();

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.DoNotSetupDatabasePermissions(); //Only required to simplify the sample setup
        persistence.SetDefaultDocumentStore(documentStore);

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
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