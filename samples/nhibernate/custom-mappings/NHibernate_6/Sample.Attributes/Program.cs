using System;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.CustomNhMappings.Attributes";
        var nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        AddAttributeMappings(nhConfiguration);

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomNhMappings.Attributes");
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

    #region AttributesConfiguration

    static void AddAttributeMappings(Configuration nhConfiguration)
    {
        var hbmSerializer = new HbmSerializer
        {
            Validate = true
        };

        using (var stream = hbmSerializer.Serialize(typeof(Program).Assembly))
        {
            nhConfiguration.AddInputStream(stream);
        }
    }

    #endregion
}