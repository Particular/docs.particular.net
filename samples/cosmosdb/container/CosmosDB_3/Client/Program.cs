using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Client";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Container.Client");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        builder.UseNServiceBus(endpointConfiguration);

        var host = builder.Build();
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
                await messageSession.Send("Samples.CosmosDB.Container.Server", startOrder);
                Console.WriteLine($"StartOrder Message sent to Server with OrderId {orderId}");
                continue;
            }
            break;
        }

        await host.StopAsync();
    }
}