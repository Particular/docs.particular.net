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

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SqlNHibernate.Receiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<SqlServerTransport>()
            .DefaultSchema("receiver")
            .UseSpecificSchema(e =>
            {
                if (e == "error" || e == "audit")
                {
                    return "dbo";
                }
                if (e == "Samples.SqlNHibernate.Sender")
                {
                    return "sender";
                }
                return null;
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