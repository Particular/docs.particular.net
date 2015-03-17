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
    class Program
    {
        public static ISessionFactory SessionFactory;

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
            SessionFactory = hibernateConfig.BuildSessionFactory();

            #endregion

            new SchemaExport(hibernateConfig).Execute(false, true, false);

            #region ReceiverConfiguration

            BusConfiguration busConfig = new BusConfiguration();
            busConfig.UseTransport<SqlServerTransport>().UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("sender")
                    .UseConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True"));

            busConfig.UsePersistence<NHibernatePersistence>();
            busConfig.EnableOutbox();

            #endregion

            busConfig.DisableFeature<SecondLevelRetries>();

            using (Bus.Create(busConfig).Start())
            {
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine();
            }
        }
    }
}