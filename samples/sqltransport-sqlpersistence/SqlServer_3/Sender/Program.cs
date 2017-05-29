using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SQLServer;
using NServiceBus.Persistence.Sql;

class Program
{
    const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
    static Random random;

    static void Main()
    {
        random = new Random();
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlTransportSqlPersistence.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlTransportSqlPersistence.Sender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region SenderConfiguration

        var connectionString = @"Data Source=.\SqlExpress;Database=shared;Integrated Security=True;Min Pool Size=2;Max Pool Size=100";
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        transport.DefaultSchema("sender");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MsSqlServer);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
            });
        persistence.Schema("sender");
        persistence.TablePrefix("");
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion

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

            var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
            var orderSubmitted = new OrderSubmitted
            {
                OrderId = orderId,
                Value = random.Next(100)
            };
            await endpointInstance.Publish(orderSubmitted)
                .ConfigureAwait(false);
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}