using System;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System.Threading;

class Program
{
    static async Task Main()
    {
        const string tablePrefix = "";

        Console.Title = "Samples.MultiTenant.Receiver";

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        endpointConfiguration.UseTransport(new LearningTransport());

        #region DisablingOutboxCleanup

        var outboxSettings = endpointConfiguration.EnableOutbox();
        outboxSettings.DisableCleanup();

        #endregion

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();

        #region ConnectionFactory

        persistence.MultiTenantConnectionBuilder(tenantIdHeaderName: "tenant_id",
            buildConnectionFromTenantData: tenantId =>
            {
                var connectionString = Connections.GetForTenant(tenantId);
                return new SqlConnection(connectionString);
            });

        #endregion

        persistence.SubscriptionSettings().DisableCache();
        persistence.TablePrefix(tablePrefix);

        var pipeline = endpointConfiguration.Pipeline;

        pipeline.Register(new StoreTenantIdBehavior(), "Stores tenant ID in the session");
        pipeline.Register(new PropagateTenantIdBehavior(), "Propagates tenant ID to outgoing messages");

        var startableEndpoint = await Endpoint.Create(endpointConfiguration)
            .ConfigureAwait(false);

        using (var connection = new SqlConnection(Connections.TenantA))
        using (var receiverDataContext = new ReceiverDataContext(connection))
        {
            await receiverDataContext.Database.EnsureCreatedAsync();
        }

        using (var connection = new SqlConnection(Connections.TenantB))
        using (var receiverDataContext = new ReceiverDataContext(connection))
        {
            await receiverDataContext.Database.EnsureCreatedAsync();
        }

        var dialect = new SqlDialect.MsSqlServer();
        var scriptDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NServiceBus.Persistence.Sql", "MsSqlServer");

        #region CreateSchema

        await ScriptRunner.Install(dialect, tablePrefix, () => new SqlConnection(Connections.TenantA), scriptDirectory,
            shouldInstallOutbox: true,
            shouldInstallSagas: true,
            shouldInstallSubscriptions: false,
            cancellationToken: CancellationToken.None);

        await ScriptRunner.Install(dialect, tablePrefix, () => new SqlConnection(Connections.TenantB), scriptDirectory,
            shouldInstallOutbox: true,
            shouldInstallSagas: true,
            shouldInstallSubscriptions: false,
            cancellationToken: CancellationToken.None);

        #endregion

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