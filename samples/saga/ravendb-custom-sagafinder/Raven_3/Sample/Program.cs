using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;
using Raven.Client.UniqueConstraints;

class Program
{

    static void Main()
    {
        var endpointName = "Samples.RavenDBCustomSagaFinder";
        Console.Title = endpointName;
        using (new RavenHost())
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            busConfiguration.EnableInstallers();

            #region config
            var documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "NServiceBus"
            };
            documentStore.RegisterListener(new UniqueConstraintsStoreListener());
            documentStore.Initialize();

            var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
            // Only required to simplify the sample setup
            persistence.DoNotSetupDatabasePermissions();
            persistence.SetDefaultDocumentStore(documentStore);

            #endregion
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                var startOrder = new StartOrder
                {
                    OrderId = "123"
                };
                bus.SendLocal(startOrder);

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}