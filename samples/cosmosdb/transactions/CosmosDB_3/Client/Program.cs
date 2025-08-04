using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Client";

        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddLogging(logging => logging.AddConsole());
            })
            .UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Transactions.Client");
                endpointConfiguration.UsePersistence<LearningPersistence>();
                endpointConfiguration.UseTransport(new LearningTransport());
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                return endpointConfiguration;
            });

        var host = hostBuilder.Build();
        await host.StartAsync();

        var messageSession = host.Services.GetRequiredService<IMessageSession>();

        Console.WriteLine("Press 'S' to send a StartOrder message to the server endpoint");
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
            if (key.Key == ConsoleKey.S)
            {
                await messageSession.Send("Samples.CosmosDB.Transactions.Server", startOrder);
                Console.WriteLine($"StartOrder Message sent to Server with OrderId {orderId}");
                continue;
            }
            break;
        }

        await host.StopAsync();
    }
}