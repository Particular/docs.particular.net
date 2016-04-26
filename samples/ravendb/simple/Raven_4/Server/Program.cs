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
        Console.Title = "Samples.RavenDB.Server";
        using (new RavenHost())
        {
            #region Config

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.RavenDB.Server");
            DocumentStore documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "RavenSampleData"
            };
            documentStore.Initialize();

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.DoNotSetupDatabasePermissions(); //Only required to simplify the sample setup
            persistence.SetDefaultDocumentStore(documentStore);

            #endregion

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpoint.Stop();
        }
    }
}