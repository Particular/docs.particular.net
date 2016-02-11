using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        using (ReceiverDataContext ctx = new ReceiverDataContext())
        {
            ctx.Database.Initialize(true);
        }

        Configuration hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });

        hibernateConfig.SetProperty("default_schema", "receiver");

        BusConfiguration busConfiguration = new BusConfiguration();
        
        #region ReceiverConfiguration

        busConfiguration
            .UseTransport<SqlServerTransport>()
            .UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("sender").UseSchema("sender"))
            .DefaultSchema("receiver");

        busConfiguration
            .UsePersistence<NHibernatePersistence>()
            .RegisterManagedSessionInTheContainer();

        busConfiguration.EnableOutbox();

        #endregion

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
