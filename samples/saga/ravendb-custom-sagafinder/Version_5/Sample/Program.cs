using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;
using Raven.Client.UniqueConstraints;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.RavenDBCustomSagaFinder";
        using (new RavenHost())
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.RavenDBCustomSagaFinder");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();

            DocumentStore documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "NServiceBus"
            };
            documentStore.RegisterListener(new UniqueConstraintsStoreListener());
            documentStore.Initialize();

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions() //Only required to simplify the sample setup
                .SetDefaultDocumentStore(documentStore);

            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                bus.SendLocal(new StartOrder
                {
                    OrderId = "123"
                });

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}

