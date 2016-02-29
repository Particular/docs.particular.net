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
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.EndpointName("Samples.RavenDBCustomSagaFinder");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            DocumentStore documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "NServiceBus"
            };
            documentStore.RegisterListener(new UniqueConstraintsStoreListener());
            documentStore.Initialize();

            endpointConfiguration.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions() //Only required to simplify the sample setup
                .SetDefaultDocumentStore(documentStore);

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
            await endpoint.SendLocal(new StartOrder
            {
                OrderId = "123"
            });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpoint.Stop();
        }
    }
}

