using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NServiceBus.NHibernate;
using NServiceBus.Persistence;
using Configuration = NHibernate.Cfg.Configuration;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";

        #region NHibernateConfiguration

        var config = CreateNHibernateConfig();
        var mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        #endregion

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var transport = new LearningTransport
        {
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        endpointConfiguration.UseTransport(transport);
        endpointConfiguration.EnableOutbox();

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(config);
        //Schema creation is controlled manually
        persistence.DisableSchemaUpdate();

        var pipeline = endpointConfiguration.Pipeline;

        #region ExtractTenantConnectionStringBehavior

        pipeline.Register(
            behavior: typeof(ExtractTenantConnectionStringBehavior),
            description: "Extracts tenant connection string based on tenant ID header.");

        #endregion

        pipeline.Register(new StoreTenantIdBehavior(), "Stores tenant ID in the session");
        pipeline.Register(new PropagateTenantIdBehavior(), "Propagates tenant ID to outgoing messages");

        var startableEndpoint = await Endpoint.Create(endpointConfiguration);

        #region CreateSchema

        var outboxScript = ScriptGenerator<MsSql2012Dialect>.GenerateOutboxScript();
        var entityScript = ScriptGenerator<MsSql2012Dialect>.GenerateOutboxScript(typeof(OrderMap));
        var sagaScript = ScriptGenerator<MsSql2012Dialect>.GenerateSagaScript<OrderLifecycleSaga>();

        SqlHelper.ExecuteSql(Connections.TenantA, sagaScript);
        SqlHelper.ExecuteSql(Connections.TenantA, entityScript);

        SqlHelper.ExecuteSql(Connections.TenantB, sagaScript);
        SqlHelper.ExecuteSql(Connections.TenantB, entityScript);

        SqlHelper.ExecuteSql(Connections.Shared, outboxScript);

        #endregion

        #region Synonyms

        var sql = @"
if exists (select * from sys.synonyms where [name] = 'OutboxRecord')
   return;

create synonym OutboxRecord FOR [NHibernateMultiTenantReceiver].[dbo].[OutboxRecord]";
        SqlHelper.ExecuteSql(Connections.TenantA, sql);
        SqlHelper.ExecuteSql(Connections.TenantB, sql);

        #endregion

        var endpointInstance = await startableEndpoint.Start();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        if (endpointInstance != null)
        {
            await endpointInstance.Stop();
        }
    }

    static Configuration CreateNHibernateConfig()
    {
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(configuration =>
        {
            #region ConnectionProvider

            configuration.ConnectionProvider<MultiTenantConnectionProvider>();

            #endregion

            configuration.Dialect<MsSql2012Dialect>();
            configuration.Driver<MicrosoftDataSqlClientDriver>();
            configuration.ConnectionString = Connections.Shared;
        });
        return hibernateConfig;
    }
}