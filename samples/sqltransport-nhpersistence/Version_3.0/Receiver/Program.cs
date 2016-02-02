using System;
using System.Threading.Tasks;
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
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
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

        #region ReceiverConfiguration

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.AuditProcessedMessagesTo("audit");
        busConfiguration.EnableInstallers();
        busConfiguration.UseTransport<SqlServerTransport>()
            .DefaultSchema("receiver")
            .UseSpecificSchema(e =>
            {
                switch (e)
                {
                    case "error":
                        return "dbo";
                    case "audit":
                        return "dbo";
                    default:
                        string schema = e.Split(new[]
                        {
                            '.'
                        }, StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
                        return schema;
                }
            });

        busConfiguration.UsePersistence<NHibernatePersistence>()
            .UseConfiguration(hibernateConfig)
            .RegisterManagedSessionInTheContainer();

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}