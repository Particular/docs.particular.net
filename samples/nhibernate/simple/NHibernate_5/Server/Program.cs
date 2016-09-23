using System;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.NHibernate.Server";

        #region Config

        var nhConfig = new Configuration();

        nhConfig.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfig.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfig.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfig.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");
        AddMappings(nhConfig);

        Configure.Serialization.Json();
        Configure.Features.Enable<Sagas>();

        var busConfiguration = Configure.With();
        busConfiguration.DefineEndpointName("Samples.NHibernate.Server")
            .DefaultBuilder()
            .UseNHibernateSagaPersister(nhConfig)
            .UseNHibernateTimeoutPersister(nhConfig, true)
            .UseNHibernateSubscriptionPersister(nhConfig)
            .UnicastBus();

        #endregion

        var startableBus = busConfiguration.CreateBus();
        var bus = startableBus.Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    static void AddMappings(Configuration nhConfiguration)
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(typeof (OrderShipped).Assembly.GetTypes());
        nhConfiguration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
    }
}