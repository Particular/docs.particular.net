using System;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client.Documents;
using Raven.Client.Exceptions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.RavenDB.Server";

        #region Config

        var endpointConfiguration = new EndpointConfiguration("Samples.RavenDB.Server");
        using (var documentStore = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = "RavenSimpleSample",
        })
        {
            documentStore.Initialize();

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.SetDefaultDocumentStore(documentStore);

            #endregion

            var outbox = endpointConfiguration.EnableOutbox();
            outbox.SetTimeToKeepDeduplicationData(TimeSpan.FromMinutes(5));

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.Transactions(TransportTransactionMode.ReceiveOnly);

            await EnsureDatabaseExists(documentStore);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }

    static async Task EnsureDatabaseExists(DocumentStore documentStore)
    {
        // create the database
        try
        {
            await documentStore.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(documentStore.Database)));
        }
        catch (ConcurrencyException)
        {
            // intentionally ignored
        }
    }
}