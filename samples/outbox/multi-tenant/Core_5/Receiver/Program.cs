using System;
using System.Data.SqlClient;
using System.IO;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
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
        Console.Title = "Samples.MultiTenant.Receiver";

        var hibernateConfig = CreateBasicNHibernateConfig();
        var mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MultiTenant.Receiver");

        #region ReceiverConfiguration

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.RegisterManagedSessionInTheContainer();
        persistence.UseConfiguration(hibernateConfig);
        persistence.UseSubscriptionStorageConfiguration(CreateBasicNHibernateConfig());
        persistence.UseTimeoutStorageConfiguration(CreateBasicNHibernateConfig());
        persistence.DisableSchemaUpdate();

        busConfiguration.EnableOutbox();

        var settingsHolder = busConfiguration.GetSettings();
        settingsHolder.Set("NHibernate.Timeouts.AutoUpdateSchema", true);
        settingsHolder.Set("NHibernate.Subscriptions.AutoUpdateSchema", true);

        #endregion

        #region ReplaceOpenSqlConnection

        busConfiguration.Pipeline.Replace("OpenSqlConnection", typeof(MultiTenantOpenSqlConnectionBehavior));

        #endregion

        #region RegisterPropagateTenantIdBehavior

        busConfiguration.Pipeline.Register<PropagateTenantIdBehavior.Registration>();

        #endregion

        var startableBus = Bus.Create(busConfiguration);

        #region CreateSchema

        CreateSchema(hibernateConfig, Connections.TenantA);
        CreateSchema(hibernateConfig, Connections.TenantB);

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
            new SchemaExport(hibernateConfig)
                .Execute(false, true, false, connection, TextWriter.Null);
        }
    }
}