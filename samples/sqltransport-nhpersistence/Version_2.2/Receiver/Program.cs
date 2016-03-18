using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlNHibernate.Receiver";
        Configuration hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });

        #region NHibernate

        hibernateConfig.SetProperty("default_schema", "receiver");

        #endregion

        ModelMapper mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        new SchemaExport(hibernateConfig).Execute(false, true, false);

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlNHibernate.Receiver");
        busConfiguration.EnableInstallers();

        #region ReceiverConfiguration

        busConfiguration.UseTransport<SqlServerTransport>()
            .DefaultSchema("receiver")
            .UseSpecificConnectionInformation(endpoint =>
            {
                if (endpoint == "error" || endpoint == "audit")
                {
                    return ConnectionInfo.Create().UseSchema("dbo");
                }
                if (endpoint == "Samples.SqlNHibernate.Sender")
                {
                    return ConnectionInfo.Create().UseSchema("sender");
                }
                return null;
            });
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .UseConfiguration(hibernateConfig)
            .RegisterManagedSessionInTheContainer();
        #endregion

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}