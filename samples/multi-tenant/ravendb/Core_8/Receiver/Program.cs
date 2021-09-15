using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.Exceptions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MultiTenant.Receiver";

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");

        using (var documentStore = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = "MultiTenantSamples",
#if NET461
            EnlistInDistributedTransactions = false
#endif
        })
        {
            #region DocumentStoreListener

            documentStore.OnBeforeStore += (sender, args) =>
            {
                if (args.Entity.GetType().Name != "OutboxRecord")
                {
                    return;
                }

                var dispatched = args.Entity.GetPropertyValue<bool>("Dispatched");
                if (dispatched)
                {
                    var dispatchedAt = args.Entity.GetPropertyValue<DateTime>("DispatchedAt");
                    var expiry = dispatchedAt.AddDays(10);
                    args.DocumentMetadata["Raven-Expiration-Date"] = expiry.ToString(CultureInfo.InvariantCulture);
                }
            };

            #endregion

            documentStore.Initialize();
            await CreateDatabase(documentStore);
            await CreateDatabase(documentStore, "A");
            await CreateDatabase(documentStore, "B");

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.SetDefaultDocumentStore(documentStore);

            endpointConfiguration.UseTransport(new LearningTransport());

            #region DetermineDatabase

            persistence.SetMessageToDatabaseMappingConvention(headers =>
            {
                return headers.TryGetValue("tenant_id", out var tenantId)
                    ? $"MultiTenantSamples-{tenantId}"
                    : "MultiTenantSamples";
            });

            #endregion

            #region DisableOutboxCleanup

            var outboxSettings = endpointConfiguration.EnableOutbox();
            outboxSettings.SetFrequencyToRunDeduplicationDataCleanup(Timeout.InfiniteTimeSpan);

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

    static async Task CreateDatabase(DocumentStore documentStore, string tenant = null)
    {
        #region CreateDatabase

        var id = tenant != null ? $"MultiTenantSamples-{tenant}" : "MultiTenantSamples";
        // create the database
        try
        {
            await documentStore.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(id)));
        }
        catch (ConcurrencyException)
        {
            // intentionally ignored
        }

        // enable document expiration
        await documentStore.Maintenance.ForDatabase(id).SendAsync(new ConfigureExpirationOperation(new ExpirationConfiguration { Disabled = false, DeleteFrequencyInSec = 10}));

        #endregion
    }
}