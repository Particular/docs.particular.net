using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

public static class Program
{
    static Random random;

    public static async Task Main()
    {
        random = new Random();

        Console.Title = "Samples.Sql.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Sender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region SenderConfiguration

        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSql;Integrated Security=True;Max Pool Size=100";
        var transport = new SqlServerTransport(connection)
        {
            DefaultSchema = "sender",
            Subscriptions = 
            {
                SubscriptionTableName = new SubscriptionTableName(
                        table: "Subscriptions", 
                        schema: "dbo")
            }
        };
        transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

        endpointConfiguration.UsePersistence<NonDurablePersistence>();

        endpointConfiguration.UseTransport(transport);

        #endregion

        SqlHelper.CreateSchema(connection, "sender");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

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
            await endpointInstance.Publish(orderSubmitted)
                .ConfigureAwait(false);
            Console.WriteLine("Published OrderSubmitted message");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}