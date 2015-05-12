using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transports.SQLServer;
using Configuration = NHibernate.Cfg.Configuration;

namespace Receiver
{
    using NServiceBus.Persistence;

    class Program
    {
        static void Main()
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

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>().UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("sender")
                    .UseConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True"));

            busConfig.UsePersistence<NHibernatePersistence>().RegisterManagedSessionInTheContainer().UseConfiguration(hibernateConfig);
            busConfig.EnableOutbox();

            #endregion

            busConfiguration.DisableFeature<SecondLevelRetries>();

            using (Bus.Create(busConfiguration).Start())
            {
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine();
            }
        }
    }
}