using System;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client.Document;
using Raven.Client.UniqueConstraints;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.RavenDBCustomSagaFinder";
        using (new RavenHost())
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.RavenDBCustomSagaFinder");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            var documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "NServiceBus"
            };
            documentStore.RegisterListener(new UniqueConstraintsStoreListener());
            documentStore.Initialize();

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            // Only required to simplify the sample setup
            persistence.DoNotSetupDatabasePermissions();
            persistence.SetDefaultDocumentStore(documentStore);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            var startOrder = new StartOrder
            {
                OrderId = "123"
            };
            await endpointInstance.SendLocal(startOrder)
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}

