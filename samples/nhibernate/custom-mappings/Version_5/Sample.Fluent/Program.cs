using System;
using System.Transactions;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{
    static void Main()
    {
        var nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        nhConfiguration = AddFluentMappings(nhConfiguration);

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomMappings");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        busConfiguration
            .UsePersistence<NHibernatePersistence>()
            .UseConfiguration(nhConfiguration);

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            using (var tx = new TransactionScope())
            {
                const int SendsNr = 15;

                for (int x = 0; x < SendsNr; x++)
                    bus.SendLocal(new StartOrder
                    {
                        OrderId = "123"
                    });

                for (int x = 0; x < SendsNr; x++)
                    bus.SendLocal(new StartOrder
                    {
                        OrderId = "456"
                    });

                bus.SendLocal(new CompleteOrder
                {
                    OrderId = "123"
                });

                tx.Complete();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }

    #region FluentConfiguration
    static Configuration AddFluentMappings(Configuration nhConfiguration)
    {
        return Fluently
            .Configure(nhConfiguration)
            .Mappings(cfg =>
            {
                cfg.FluentMappings.AddFromAssemblyOf<OrderSagaDataFluent>();
            })
            .BuildConfiguration();
    }
    #endregion
}
