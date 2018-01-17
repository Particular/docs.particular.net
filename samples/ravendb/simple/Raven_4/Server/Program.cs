using System;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client.Document;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.RavenDB.Server";
        #region Config

        var endpointConfiguration = new EndpointConfiguration("Samples.RavenDB.Server");
        using (var documentStore = new DocumentStore
        {
            Url = "http://localhost:8080",
            DefaultDatabase = "RavenSimpleSample",
            EnlistInDistributedTransactions = false
        })
        {
            documentStore.Initialize();

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            // Only required to simplify the sample setup
            persistence.DoNotSetupDatabasePermissions();
            persistence.SetDefaultDocumentStore(documentStore);

            #endregion

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}