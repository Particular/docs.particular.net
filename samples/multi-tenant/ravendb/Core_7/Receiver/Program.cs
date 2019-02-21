using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Abstractions.Data;
using Raven.Client.Document;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MultiTenant.Receiver";

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");

        using (var documentStore = new DocumentStore
        {
            Url = "http://localhost:8080",
            DefaultDatabase = "MultiTenantSamples",

#if NET461
            EnlistInDistributedTransactions = false
#endif
        })
        {
            documentStore.RegisterListener(new MarkForExpiryListener());
            documentStore.Initialize();
            CreateDatabase(documentStore, "A");
            CreateDatabase(documentStore, "B");

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            // Only required to simplify the sample setup
            persistence.DoNotSetupDatabasePermissions();
            persistence.DisableSubscriptionVersioning();
            persistence.SetDefaultDocumentStore(documentStore);

            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.EnableOutbox();

            #region DetermineDatabase

            persistence.SetMessageToDatabaseMappingConvention(headers =>
            {
                return headers.TryGetValue("tenant_id", out var tenantId)
                    ? $"MultiTenantSamples-{tenantId}"
                    : "MultiTenantSamples";
            });

            #endregion

            #region DisableOutboxCleanup

            endpointConfiguration.SetFrequencyToRunDeduplicationDataCleanup(Timeout.InfiniteTimeSpan);

            #endregion

            var pipeline = endpointConfiguration.Pipeline;

            pipeline.Register(new StoreTenantIdBehavior(), "Stores tenant ID in the session");
            pipeline.Register(new PropagateTenantIdBehavior(), "Propagates tenant ID to outgoing messages");

            var startableEndpoint = await Endpoint.Create(endpointConfiguration)
                .ConfigureAwait(false);


            var endpointInstance = await startableEndpoint.Start()
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            if (endpointInstance != null)
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
    }

    static void CreateDatabase(DocumentStore documentStore, string tenant)
    {
        var globalAdmin = documentStore.DatabaseCommands.GlobalAdmin;

        #region CreateDatabase

        var existingDbs = globalAdmin.GetDatabaseNames(100);
        var id = $"MultiTenantSamples-{tenant}";
        if (existingDbs.Contains(id))
        {
            return;
        }

        globalAdmin.CreateDatabase(new DatabaseDocument
        {
            Id = id,
            Settings =
            {
                { "Raven/ActiveBundles", "DocumentExpiration" },
                { "Raven/DataDir", $@"~\Databases\{id}" }
            }
        });

        #endregion
    }
}