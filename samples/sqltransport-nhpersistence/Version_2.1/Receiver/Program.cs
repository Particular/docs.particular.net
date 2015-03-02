using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Util;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transports.SQLServer;
using Configuration = NHibernate.Cfg.Configuration;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var hibernateConfig = new Configuration();
            hibernateConfig.DataBaseIntegration(x =>
            {                
                x.ConnectionStringName = "NServiceBus/Persistence";
                x.Dialect<MsSql2012Dialect>();
            });
            #region NHibernate
            hibernateConfig.SetProperty("default_schema", "receiver");
            #endregion

            var mapper = new ModelMapper();
            mapper.AddMapping<OrderMap>();
            hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            new SchemaExport(hibernateConfig).Execute(false, true, false);

            #region ReceiverConfiguration
            
            var busConfig = new BusConfiguration();
            busConfig.UseTransport<SqlServerTransport>().DefaultSchema("receiver")
                .UseSpecificConnectionInformation(endpoint =>
                {
                    var schema = endpoint.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
                    return ConnectionInfo.Create().UseSchema(schema);
                });
            busConfig.UsePersistence<NHibernatePersistence>().UseConfiguration(hibernateConfig);
            #endregion

            using (Bus.Create(busConfig).Start())
            {
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine();
            }
        }
    }
}
