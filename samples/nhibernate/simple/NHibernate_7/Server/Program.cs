﻿using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.NHibernate.Server";

        #region Config

        var endpointConfiguration = new EndpointConfiguration("Samples.NHibernate.Server");
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();

        var nhConfig = new Configuration();
        nhConfig.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfig.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfig.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfig.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        AddMappings(nhConfig);

        persistence.UseConfiguration(nhConfig);

        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static void AddMappings(Configuration nhConfiguration)
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(typeof (OrderShipped).Assembly.GetTypes());
        nhConfiguration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
    }
}