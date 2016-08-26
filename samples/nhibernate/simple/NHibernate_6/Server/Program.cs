using System;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NServiceBus;
using NServiceBus.Persistence;
using Server;
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
        var nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        AddMappings(nhConfiguration);

        persistence.UseConfiguration(nhConfiguration).RegisterManagedSessionInTheContainer();

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        var bus = Bus.Create(busConfiguration).Start();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        bus.Dispose();
    }

    private static void AddMappings(Configuration nhConfiguration)
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(typeof (OrderShipped).Assembly.GetTypes());
        nhConfiguration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
    }
}
