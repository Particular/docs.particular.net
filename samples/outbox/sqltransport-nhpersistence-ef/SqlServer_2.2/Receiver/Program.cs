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
        Console.Title = "Samples.SQLNHibernateOutboxEF.Receiver";
        using (var receiverDataContext = new ReceiverDataContext())
        {
            receiverDataContext.Database.Initialize(true);
        }

        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(
            dataBaseIntegration: configurationProperties =>
            {
                configurationProperties.ConnectionStringName = "NServiceBus/Persistence";
                configurationProperties.Dialect<MsSql2012Dialect>();
            });

        hibernateConfig.SetProperty("default_schema", "receiver");

        var busConfiguration = new BusConfiguration();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EndpointName("Samples.SQLNHibernateOutboxEF.Receiver");

        #region ReceiverConfiguration

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        var connectionInfo = EndpointConnectionInfo.For("Samples.SQLNHibernateOutboxEF.Sender")
            .UseSchema("sender");
        transport.UseSpecificConnectionInformation(connectionInfo);
        transport.DefaultSchema("receiver");

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.RegisterManagedSessionInTheContainer();

        busConfiguration.EnableOutbox();

        #endregion

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}