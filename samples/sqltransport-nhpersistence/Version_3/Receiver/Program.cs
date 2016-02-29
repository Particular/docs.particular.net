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

        #region ReceiverConfiguration

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<SqlServerTransport>()
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

        endpointConfiguration.UsePersistence<NHibernatePersistence>()
            .UseConfiguration(hibernateConfig)
            .RegisterManagedSessionInTheContainer();

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
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