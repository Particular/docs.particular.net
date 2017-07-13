using System;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.CustomNhMappings.Loquacious";
        var nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        nhConfiguration = AddFluentMappings(nhConfiguration);

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomNhMappings.Loquacious");
        busConfiguration.EnableInstallers();

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(nhConfiguration);

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var startOrder = new StartOrder
            {
                OrderId = "123"
            };
            bus.SendLocal(startOrder);
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