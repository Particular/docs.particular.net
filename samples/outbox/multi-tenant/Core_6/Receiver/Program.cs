using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;
using Configuration = NHibernate.Cfg.Configuration;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MultiTenant.Receiver";

        var sharedDatabaseConfiguration = CreateBasicNHibernateConfig();

        var tenantDatabasesConfiguration = CreateBasicNHibernateConfig();
        var mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        tenantDatabasesConfiguration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        routing.RegisterPublisher(
            eventType: typeof(OrderSubmitted),
            publisherEndpoint: "Samples.MultiTenant.Sender");

        #region ReceiverConfiguration

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(tenantDatabasesConfiguration);
        persistence.UseSubscriptionStorageConfiguration(sharedDatabaseConfiguration);
        persistence.UseTimeoutStorageConfiguration(sharedDatabaseConfiguration);
        persistence.DisableSchemaUpdate();

        endpointConfiguration.EnableOutbox();

        var settings = endpointConfiguration.GetSettings();
        settings.Set("NHibernate.Timeouts.AutoUpdateSchema", true);
        settings.Set("NHibernate.Subscriptions.AutoUpdateSchema", true);

        #endregion

        ReplaceOpenSqlConnection(endpointConfiguration);

        RegisterPropagateTenantIdBehavior(endpointConfiguration);


        var startableEndpoint = await Endpoint.Create(endpointConfiguration)
            .ConfigureAwait(false);

        #region CreateSchema

        CreateSchema(tenantDatabasesConfiguration, Connections.TenantA);
        CreateSchema(tenantDatabasesConfiguration, Connections.TenantB);

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

    static void RegisterPropagateTenantIdBehavior(EndpointConfiguration endpointConfiguration)
    {
        #region RegisterPropagateTenantIdBehavior

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register<PropagateOutgoingTenantIdBehavior.Registration>();
        pipeline.Register<PropagateIncomingTenantIdBehavior.Registration>();

        #endregion
    }

    static void ReplaceOpenSqlConnection(EndpointConfiguration endpointConfiguration)
    {
        #region ReplaceOpenSqlConnection

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register<ExtractTenantConnectionStringBehavior.Registration>();

        #endregion
    }

    static Configuration CreateBasicNHibernateConfig()
    {
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            #region ConnectionProvider

            x.ConnectionProvider<MultiTenantConnectionProvider>();

            #endregion

            x.Dialect<MsSql2012Dialect>();
            x.ConnectionString = Connections.Default;
        });
        return hibernateConfig;
    }

    static void CreateSchema(Configuration hibernateConfig, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var schemaExport = new SchemaExport(hibernateConfig);
            schemaExport.Execute(false, true, false, connection, TextWriter.Null);
        }
    }
}