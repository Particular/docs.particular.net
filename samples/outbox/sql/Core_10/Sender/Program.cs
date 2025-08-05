using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

Console.Title = "Sender";
var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(x =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlOutbox.Sender");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region SenderConfiguration

        //for local instance or SqlExpress
        // string connectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlOutbox;Trusted_Connection=True;MultipleActiveResultSets=true";
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlOutbox;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

        var transport = new SqlServerTransport(connectionString)
        {
            DefaultSchema = "sender",
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
        transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

        endpointConfiguration.UseTransport(transport);

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () => new SqlConnection(connectionString)
        );
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("sender");
        persistence.TablePrefix("");

        transport.Subscriptions.DisableCaching = true;
        transport.Subscriptions.SubscriptionTableName = new SubscriptionTableName(
            table: "Subscriptions",
            schema: "dbo"
        );

        endpointConfiguration.EnableOutbox();

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        #endregion

        SqlHelper.CreateSchema(connectionString, "sender");
        Console.WriteLine("Press enter to send a message");
        return endpointConfiguration;
    })
    .Build();

await host.StartAsync();

var messageSession = host.Services.GetService<IMessageSession>();
var random = new Random();

while (true)
{
    if (!Console.KeyAvailable)
    {
        await Task.Delay(100);
        continue;
    }
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var orderSubmitted = new OrderSubmitted(
        OrderId: Guid.NewGuid(),
        Value: random.Next(100)
    );

    await messageSession.Publish(orderSubmitted);
}

await host.StopAsync();