using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;

class Program
{

    static void Main()
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

        using (IBus bus = Bus.Create(busConfiguration).Start())
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
                    bus.SendLocal(new StartOrder
                    {
                        OrderId = 10,
                        ItemCount = 5
                    });
                    continue;
                }

                if (key.Key == ConsoleKey.I)
                {
                    bus.SendLocal(new IncrementOrder
                    {
                        OrderId = 10,
                    });
                    continue;
                }
                return;
            }
        }
    }
}