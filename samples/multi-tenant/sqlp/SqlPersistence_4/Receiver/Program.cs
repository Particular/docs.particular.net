using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Pipeline;

class Program
{
    static async Task Main()
    {
        const string tablePrefix = "";

        Console.Title = "Samples.MultiTenant.Receiver";

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableOutbox();

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();

        #region ConnectionFactory

        persistence.ConnectionBuilder(MultiTenantConnectionFactory.GetConnection);

        #endregion

        persistence.SubscriptionSettings().DisableCache();
        persistence.TablePrefix(tablePrefix);

        var pipeline = endpointConfiguration.Pipeline;

        #region ExtractTenantConnectionStringBehavior

        pipeline.Register(
            behavior: typeof(ExtractTenantConnectionStringBehavior), 
            description: "Extracts tenant connection string based on tenant ID header.");

        #endregion

        pipeline.Register(new StoreTenantIdBehavior(), "Stores tenant ID in the session");
        pipeline.Register(new PropagateTenantIdBehavior(), "Propagates tenant ID to outgoing messages");

        var startableEndpoint = await Endpoint.Create(endpointConfiguration)
            .ConfigureAwait(false);

        Database.SetInitializer(new CreateDatabaseIfNotExists<ReceiverDataContext>());

        using (var connection = new SqlConnection(Connections.TenantA))
        using (var receiverDataContext = new ReceiverDataContext(connection))
        {
            receiverDataContext.Database.Initialize(true);
        }

        using (var connection = new SqlConnection(Connections.TenantB))
        using (var receiverDataContext = new ReceiverDataContext(connection))
        {
            receiverDataContext.Database.Initialize(true);
        }

        var dialect = new SqlDialect.MsSqlServer();
        var scriptDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NServiceBus.Persistence.Sql", "MsSqlServer");
        
        #region CreateSchema

        await ScriptRunner.Install(dialect, tablePrefix, () => new SqlConnection(Connections.TenantA), scriptDirectory, 
            shouldInstallOutbox: false, 
            shouldInstallSagas: true, 
            shouldInstallSubscriptions: false, 
            shouldInstallTimeouts: false);

        await ScriptRunner.Install(dialect, tablePrefix, () => new SqlConnection(Connections.TenantB), scriptDirectory, 
            shouldInstallOutbox: false, 
            shouldInstallSagas: true, 
            shouldInstallSubscriptions: false, 
            shouldInstallTimeouts: false);

        await ScriptRunner.Install(dialect, tablePrefix, () => new SqlConnection(Connections.Shared), scriptDirectory, 
            shouldInstallOutbox: true, 
            shouldInstallSagas: false, 
            shouldInstallSubscriptions: false, 
            shouldInstallTimeouts: true);

        #endregion

        #region Synonyms

        var sql = @"
if exists (select * from sys.synonyms where [name] = 'OutboxData')
   return;

create synonym OutboxData FOR [SqlMultiTenantReceiver].[dbo].[OutboxData]";
        SqlHelper.ExecuteSql(Connections.TenantA, sql);
        SqlHelper.ExecuteSql(Connections.TenantB, sql);

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