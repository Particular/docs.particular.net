using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");

        using var documentStore = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = "MultiTenantSamples",
        };

        documentStore.Initialize();
        await CreateDatabase(documentStore);
        await CreateTenantDatabase(documentStore, "A");
        await CreateTenantDatabase(documentStore, "B");

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(documentStore);

        var transport = new LearningTransport
        {
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        endpointConfiguration.UseTransport(transport);
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var outbox = endpointConfiguration.EnableOutbox();

        #region DetermineDatabase

        persistence.SetMessageToDatabaseMappingConvention(headers =>
        {
            return headers.TryGetValue("tenant_id", out var tenantId)
                ? $"MultiTenantSamples-{tenantId}"
                : "MultiTenantSamples";
        });

        #endregion

        var pipeline = endpointConfiguration.Pipeline;

        pipeline.Register(new StoreTenantIdBehavior(), "Stores tenant ID in the session");
        pipeline.Register(new PropagateTenantIdBehavior(), "Propagates tenant ID to outgoing messages");

        var startableEndpoint = await Endpoint.Create(endpointConfiguration);


        var endpointInstance = await startableEndpoint.Start();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        if (endpointInstance != null)
        {
            await endpointInstance.Stop();
        }
    }

    static async Task CreateDatabase(IDocumentStore documentStore)
    {
        try
        {
            await documentStore.Maintenance.ForDatabase(documentStore.Database).SendAsync(new GetStatisticsOperation());
        }
        catch (DatabaseDoesNotExistException)
        {
            try
            {
                await documentStore.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(documentStore.Database)));
            }
            catch (ConcurrencyException)
            {
            }
        }
    }

    static async Task CreateTenantDatabase(DocumentStore documentStore, string tenant)
    {
        #region CreateDatabase

        var id = $"MultiTenantSamples-{tenant}";
        try
        {
            await documentStore.Maintenance.ForDatabase(id).SendAsync(new GetStatisticsOperation());
        }
        catch (DatabaseDoesNotExistException)
        {
            try
            {
                await documentStore.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(id)));
            }
            catch (ConcurrencyException)
            {
            }
        }

        #endregion
    }
}