using System;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
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
        Console.Title = "Samples.CustomNhMappings.Loquacious";
        var nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        nhConfiguration = AddFluentMappings(nhConfiguration);

        var endpointConfiguration = new EndpointConfiguration("Samples.CustomNhMappings.Loquacious");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<LearningTransport>();

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(nhConfiguration);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var startOrder = new StartOrder
        {
            OrderId = "123"
        };
        await endpointInstance.SendLocal(startOrder)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
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