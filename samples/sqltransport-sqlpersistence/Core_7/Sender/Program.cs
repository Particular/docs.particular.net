using System;
using NServiceBus;

Console.Title = "Samples.Sql.Sender";

var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Sender");
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.EnableInstallers();

#region SenderConfiguration

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSql;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSql;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
transport.ConnectionString(connectionString);
transport.DefaultSchema("sender");
transport.UseSchemaForQueue("error", "dbo");
transport.UseSchemaForQueue("audit", "dbo");

endpointConfiguration.UsePersistence<InMemoryPersistence>();

var subscriptions = transport.SubscriptionSettings();
subscriptions.SubscriptionTableName(
    tableName: "Subscriptions",
    schemaName: "dbo");

#endregion

await SqlHelper.CreateSchema(connectionString, "sender");

var endpointInstance = await Endpoint.Start(endpointConfiguration)
    .ConfigureAwait(false);
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