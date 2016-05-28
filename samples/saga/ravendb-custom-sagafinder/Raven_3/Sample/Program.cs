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
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.RavenDBCustomSagaFinder");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();

            var documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "NServiceBus"
            };
            documentStore.RegisterListener(new UniqueConstraintsStoreListener());
            documentStore.Initialize();

            var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
            //Only required to simplify the sample setup
            persistence.DoNotSetupDatabasePermissions();
            persistence.SetDefaultDocumentStore(documentStore);

            using (var bus = Bus.Create(busConfiguration).Start())
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

