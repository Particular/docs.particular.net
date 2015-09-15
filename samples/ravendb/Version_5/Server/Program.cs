using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;
using Raven.Client.UniqueConstraints;

class Program
{

    static void Main()
    {
        using (new RavenHost())
        {
            #region Config

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.RavenDB.Server");
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

            #endregion

            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();

            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}