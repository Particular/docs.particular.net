using System;
using System.IO;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlNHibernate.Receiver";
        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlNHibernate;Integrated Security=True; Max Pool Size=100";
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connection;
            x.Dialect<MsSql2012Dialect>();
        });

        #region NHibernate

        hibernateConfig.SetProperty("default_schema", "receiver");

        #endregion

        SqlHelper.CreateSchema(connection, "receiver");
        var mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        new SchemaExport(hibernateConfig).Execute(false, true, false);

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlNHibernate.Receiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.DefaultSchema("receiver");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");
        transport.UseSchemaForQueue("Samples.SqlNHibernate.Sender", "sender");

        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(OrderAccepted), "Samples.SqlNHibernate.Sender");
        routing.RegisterPublisher(typeof(OrderSubmitted).Assembly, "Samples.SqlNHibernate.Sender");

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(hibernateConfig);

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}