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
        Console.Title = "Samples.CustomNhMappings.XmlMapping";
        var nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        AddMappingsFromFilesystem(nhConfiguration);

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomNhMappings.XmlMapping");
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

    #region AddMappingsFromFilesystem
    static void AddMappingsFromFilesystem(Configuration nhConfiguration)
    {
        var directory = Directory.GetCurrentDirectory();
        var hmbFiles = Directory.GetFiles(directory, "*.hbm.xml", SearchOption.TopDirectoryOnly);

        foreach (var file in hmbFiles)
        {
            nhConfiguration.AddFile(file);
        }
    }
    #endregion
}
