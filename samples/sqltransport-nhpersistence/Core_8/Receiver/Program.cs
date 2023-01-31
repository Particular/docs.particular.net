using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SqlServer;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlNHibernate.Receiver";
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlNHibernate;Integrated Security=True;Max Pool Size=100;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlNHibernate;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connectionString;
            x.Dialect<MsSql2012Dialect>();
            x.Driver<MicrosoftDataSqlClientDriver>();
        });

        #region NHibernate

        hibernateConfig.SetProperty("default_schema", "receiver");

        #endregion

        await SqlHelper.CreateSchema(connectionString, "receiver");
        var mapper = new ModelMapper();
        mapper.AddMapping<OrderMap>();
        hibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        await new SchemaExport(hibernateConfig).ExecuteAsync(false, true, false);

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlNHibernate.Receiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        var transport = new SqlServerTransport(connectionString)
        {
            DefaultSchema = "receiver"
        };
        transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("Samples.SqlNHibernate.Sender", "sender");
        transport.Subscriptions.SubscriptionTableName = new SubscriptionTableName("Subscriptions", "dbo");
        transport.TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive;

        var routing = endpointConfiguration.UseTransport(transport);
        routing.RouteToEndpoint(typeof(OrderAccepted), "Samples.SqlNHibernate.Sender");

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