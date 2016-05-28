using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Features;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;
using Configuration = NHibernate.Cfg.Configuration;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MultiTenant.Receiver";

        var sharedDatabaseConfiguration = CreateBasicNHibernateConfig();

        var tenantDatabasesConfiguration = CreateBasicNHibernateConfig();
        var mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        tenantDatabasesConfiguration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ReceiverConfiguration

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(tenantDatabasesConfiguration);
        persistence.UseSubscriptionStorageConfiguration(sharedDatabaseConfiguration);
        persistence.UseTimeoutStorageConfiguration(sharedDatabaseConfiguration);
        persistence.DisableSchemaUpdate();

        endpointConfiguration.EnableOutbox();

        var settingsHolder = endpointConfiguration.GetSettings();
        settingsHolder.Set("NHibernate.Timeouts.AutoUpdateSchema", true);
        settingsHolder.Set("NHibernate.Subscriptions.AutoUpdateSchema", true);

        #endregion

        #region ReplaceOpenSqlConnection

        endpointConfiguration.Pipeline.Register<ExtractTenantConnectionStringBehavior.Registration>();

        #endregion

        #region RegisterPropagateTenantIdBehavior

        endpointConfiguration.Pipeline.Register<PropagateOutgoingTenantIdBehavior.Registration>();
        endpointConfiguration.Pipeline.Register<PropagateIncomingTenantIdBehavior.Registration>();


        #endregion

        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        #region CreateSchema

        #endregion

        var startableEndpoint = await Endpoint.Create(endpointConfiguration)
            .ConfigureAwait(false);
        IEndpointInstance endpointInstance = null;

        CreateSchema(tenantDatabasesConfiguration, "A");
        CreateSchema(tenantDatabasesConfiguration, "B");

        try
        {
            endpointInstance = await startableEndpoint.Start()
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            if (endpointInstance != null)
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
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
            x.ConnectionStringName = "NServiceBus/Persistence";
        });
        return hibernateConfig;
    }

    static void CreateSchema(Configuration hibernateConfig, string tenantId)
    {
        var connectionString = ConfigurationManager.ConnectionStrings[tenantId].ConnectionString;
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            new SchemaExport(hibernateConfig)
                .Execute(false, true, false, connection, TextWriter.Null);
        }
    }
}