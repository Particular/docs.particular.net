using System;
using System.IO;
using NHibernate.Cfg;
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

        AddMappingsFromFilesystem(nhConfiguration);

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomNhMappings.XmlMapping");
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

    #region AddMappingsFromFilesystem
    static void AddMappingsFromFilesystem(Configuration nhConfiguration)
    {
        string folder = Directory.GetCurrentDirectory();
        string[] hmbFiles = Directory.GetFiles(folder, "*.hbm.xml", SearchOption.TopDirectoryOnly);

        foreach (string file in hmbFiles)
        {
            nhConfiguration.AddFile(file);
        }
    }
    #endregion
}
