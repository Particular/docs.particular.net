using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
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

            IEndpointInstance endpoint = await Endpoint.Start( busConfiguration );
            try {
                IBusSession busSession = endpoint.CreateBusSession( );
                Console.WriteLine( "Press any key to exit" );
                Console.ReadKey( );
            }
            finally {
                await endpoint.Stop( );
            }
        }
    }
}