using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.RavenDB.Server";
        using (new RavenHost())
        {
            #region Config

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.RavenDB.Server");
            DocumentStore documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "RavenSampleData"
            };
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