using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Persistence;
using Configuration = NHibernate.Cfg.Configuration;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SQLNHibernateOutbox.Receiver";
        #region NHibernate

        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = @"Data Source=.\SqlExpress;Database=nservicebus;Integrated Security=True";
            x.Dialect<MsSql2012Dialect>();
        });
        var mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        #endregion

        new SchemaExport(hibernateConfig).Execute(false, true, false);

        var endpointConfiguration = new EndpointConfiguration("Samples.SQLNHibernateOutbox.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        #region ReceiverConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(@"Data Source=.\SqlExpress;Database=nservicebus;Integrated Security=True");
        
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(hibernateConfig);

        endpointConfiguration.EnableOutbox();

        #endregion

        transport.Routing().RouteToEndpoint(typeof(OrderAccepted).Assembly, "Samples.SQLNHibernateOutbox.Sender");

        #region RetriesConfiguration

        endpointConfiguration.Recoverability()
            .Immediate(immediate => immediate.NumberOfRetries(0))
            .Delayed(delayed => delayed.NumberOfRetries(0));

        #endregion

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}