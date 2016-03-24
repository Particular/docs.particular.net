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
using NServiceBus.Settings;
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

        Configuration hibernateConfig = CreateBasicNHibernateConfig();
        ModelMapper mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        
        #region ReceiverConfiguration
        
        endpointConfiguration.UsePersistence<NHibernatePersistence>()
            .UseConfiguration(hibernateConfig)
            .UseSubscriptionStorageConfiguration(CreateBasicNHibernateConfig())
            .UseTimeoutStorageConfiguration(CreateBasicNHibernateConfig())
            .DisableSchemaUpdate();

        SettingsHolder settingsHolder = endpointConfiguration.GetSettings();
        settingsHolder.Set("NHibernate.Timeouts.AutoUpdateSchema", true);
        settingsHolder.Set("NHibernate.Subscriptions.AutoUpdateSchema", true);

        #endregion

        #region ReplaceOpenSqlConnection

        endpointConfiguration.Pipeline.Register<MultiTenantOpenSqlConnectionBehavior.Registration>();

        #endregion

        #region RegisterPropagateTenantIdBehavior

        endpointConfiguration.Pipeline.Register<PropagateOutgoingTenantIdBehavior.Registration>();
        endpointConfiguration.Pipeline.Register<PropagateIncomingTenantIdBehavior.Registration>();
        

        #endregion

        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        #region CreateSchema

        #endregion

        IStartableEndpoint startableEndpoint = await Endpoint.Create(endpointConfiguration);
        IEndpointInstance endpoint = null;

        CreateSchema(hibernateConfig, "A");
        CreateSchema(hibernateConfig, "B");

        try
        {
            endpoint = await startableEndpoint.Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            if (endpoint != null)
            {
                await endpoint.Stop();
            }
        }
    }

    static Configuration CreateBasicNHibernateConfig()
    {
        Configuration hibernateConfig = new Configuration();
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
        string connectionString = ConfigurationManager.ConnectionStrings[tenantId].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            new SchemaExport(hibernateConfig)
                .Execute(false, true, false, connection, TextWriter.Null);
        }
    }
}