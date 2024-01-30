using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SqlServer;

Console.Title = "Samples.SqlNHibernate.Sender";

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlNHibernate;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlNHibernate;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
var endpointConfiguration = new EndpointConfiguration("Samples.SqlNHibernate.Sender");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.EnableInstallers();

var hibernateConfig = new Configuration();
hibernateConfig.DataBaseIntegration(x =>
{
    x.ConnectionString = connectionString;
    x.Dialect<MsSql2012Dialect>();
    x.Driver<MicrosoftDataSqlClientDriver>();
});
hibernateConfig.SetProperty("default_schema", "sender");

#region SenderConfiguration

var transport = new SqlServerTransport(connectionString)
{
    DefaultSchema = "sender",
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
};
transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");
transport.Subscriptions.SubscriptionTableName = new SubscriptionTableName("Subscriptions", "dbo");

var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
persistence.UseConfiguration(hibernateConfig);

#endregion

var routing = endpointConfiguration.UseTransport(transport);
routing.RouteToEndpoint(typeof(OrderAccepted), "Samples.SqlNHibernate.Sender");

await SqlHelper.CreateSchema(connectionString, "sender");

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press enter to send a message");
Console.WriteLine("Press any key to exit");

var random = new Random();

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var orderSubmitted = new OrderSubmitted
    {
        OrderId = Guid.NewGuid(),
        Value = random.Next(100)
    };

    await endpointInstance.Publish(orderSubmitted);

    Console.WriteLine("Published OrderSubmitted message");
}
await endpointInstance.Stop();