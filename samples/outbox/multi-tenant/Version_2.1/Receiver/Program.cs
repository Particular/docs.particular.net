using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Features;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;
using NServiceBus.Pipeline;
using NServiceBus.Unicast;
using Configuration = NHibernate.Cfg.Configuration;

class Program
{
    internal static PipelineExecutor PipelineExecutor;

    static void Main()
    {
        Console.Title = "Samples.SQLNHibernateOutbox.Receiver";

        Configuration hibernateConfig = CreateBasicNHibernateConfig();
        ModelMapper mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.UseSerialization<JsonSerializer>();

        #region ReceiverConfiguration

        busConfiguration.UsePersistence<NHibernatePersistence>()
            .RegisterManagedSessionInTheContainer()
            .UseConfiguration(hibernateConfig)
            .UseSubscriptionStorageConfiguration(CreateBasicNHibernateConfig())
            .UseTimeoutStorageConfiguration(CreateBasicNHibernateConfig())
            .DisableSchemaUpdate();

        busConfiguration.EnableOutbox();

        busConfiguration.GetSettings().Set("NHibernate.Timeouts.AutoUpdateSchema", true);
        busConfiguration.GetSettings().Set("NHibernate.Subscriptions.AutoUpdateSchema", true);

        #endregion

        #region ReplaceOpenSqlConnection

        busConfiguration.Pipeline.Replace("OpenSqlConnection", typeof(MultiTenantOpenSqlConnectionBehavior));

        #endregion

        #region RegisterPropagateTenantIdBehavior

        busConfiguration.Pipeline.Register<PropagateTenantIdBehavior.Registration>();

        #endregion

        busConfiguration.DisableFeature<SecondLevelRetries>();

        #region CreateSchema

        IStartableBus startableBus = Bus.Create(busConfiguration);

        CreateSchema(hibernateConfig, "A");
        CreateSchema(hibernateConfig, "B");

        #endregion

        #region CapturePipelineExecutor

        PipelineExecutor = ((UnicastBus) startableBus).Builder.Build<PipelineExecutor>();

        #endregion

        using (startableBus.Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }

    private static Configuration CreateBasicNHibernateConfig()
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

    private static void CreateSchema(Configuration hibernateConfig, string tenantId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings[tenantId].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            new SchemaExport(hibernateConfig).Execute(false, true, false, connection, TextWriter.Null);
        }
    }
}