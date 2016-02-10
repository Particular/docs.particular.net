using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Persistence;
using Configuration = NHibernate.Cfg.Configuration;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region NHibernate

        Configuration hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });
        ModelMapper mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        #endregion

        new SchemaExport(hibernateConfig).Execute(false, true, false);

        #region ReceiverConfiguration

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.UseTransport<SqlServerTransport>();

        endpointConfiguration.UsePersistence<NHibernatePersistence>()
            .RegisterManagedSessionInTheContainer()
            .UseConfiguration(hibernateConfig);
        endpointConfiguration.EnableOutbox();

        endpointConfiguration.SendFailedMessagesTo(@"Data Source=.\SQLEXPRESS;Initial Catalog=shared;Integrated Security=True");

        #endregion

        endpointConfiguration.DisableFeature<SecondLevelRetries>();

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