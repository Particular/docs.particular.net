using System;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.NHibernate.Server";

        #region Config

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.NHibernate.Server");

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        var nhConfig = new Configuration();

        nhConfig.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfig.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfig.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfig.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        AddMappings(nhConfig);

        persistence.UseConfiguration(nhConfig).RegisterManagedSessionInTheContainer();

        #endregion

        busConfiguration.EnableInstallers();

        var bus = Bus.Create(busConfiguration).Start();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        bus.Dispose();
    }

    static void AddMappings(Configuration nhConfiguration)
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(typeof (OrderShipped).Assembly.GetTypes());
        nhConfiguration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
    }
}