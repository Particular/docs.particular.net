using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlPersistence.Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.Client");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'S' to send a StartOrder message to the SqlServer endpoint");
        Console.WriteLine("Press 'M' to send a StartOrder message to the MySql endpoint");
        Console.WriteLine("Press 'O' to send a StartOrder message to the Oracle endpoint");
        Console.WriteLine("Press 'P' to send a StartOrder message to the PostgreSql endpoint");

        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderId = Guid.NewGuid();
            var startOrder = new StartOrder
            {
                OrderId = orderId
            };
            if (key.Key == ConsoleKey.M)
            {
                await endpointInstance.Send("Samples.SqlPersistence.EndpointMySql", startOrder)
                    .ConfigureAwait(false);
                Console.WriteLine($"StartOrder Message sent to EndpointMySql with OrderId {orderId}");
                continue;
            }
            if (key.Key == ConsoleKey.S)
            {
                await endpointInstance.Send("Samples.SqlPersistence.EndpointSqlServer", startOrder)
                    .ConfigureAwait(false);
                Console.WriteLine($"StartOrder Message sent to EndpointSqlServer with OrderId {orderId}");
                continue;
            }
            if (key.Key == ConsoleKey.O)
            {
                await endpointInstance.Send("EndpointOracle", startOrder)
                    .ConfigureAwait(false);
                Console.WriteLine($"StartOrder Message sent to EndpointOracle with OrderId {orderId}");
                continue;
            }
            if (key.Key == ConsoleKey.P)
            {
                await endpointInstance.Send("EndpointPostgreSql", startOrder)
                    .ConfigureAwait(false);
                Console.WriteLine($"StartOrder Message sent to EndpointPostgreSql with OrderId {orderId}");
                continue;
            }
            break;
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}