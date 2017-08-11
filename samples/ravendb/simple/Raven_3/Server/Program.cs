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

            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.RavenDB.Server");
            using (var documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "RavenSampleData"
            })
            {
                documentStore.Initialize();

                var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
                // Only required to simplify the sample setup
                persistence.DoNotSetupDatabasePermissions();
                persistence.SetDefaultDocumentStore(documentStore);

                #endregion

                busConfiguration.EnableInstallers();

                using (var bus = Bus.Create(busConfiguration).Start())
                {
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                }
            }
        }
    }
}