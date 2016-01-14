using System;
using System.IO;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{
    static void Main()
    {
        Configuration nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        AddAttributeMappings(nhConfiguration);

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomNhMappings.Attributes");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        busConfiguration
            .UsePersistence<NHibernatePersistence>()
            .UseConfiguration(nhConfiguration);

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new StartOrder
            {
                OrderId = "123"
            });

            bus.SendLocal(new CompleteOrder
            {
                OrderId = "123"
            });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }

    #region AttributesConfiguration

    static void AddAttributeMappings(Configuration nhConfiguration)
    {
        HbmSerializer hbmSerializer = new HbmSerializer
        {
            Validate = true
        };

        using (MemoryStream stream = hbmSerializer.Serialize(typeof(Program).Assembly))
        {
            nhConfiguration.AddInputStream(stream);
        }
    }

    #endregion
}