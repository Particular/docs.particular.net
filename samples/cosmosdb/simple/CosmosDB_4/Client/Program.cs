using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        await host.StartAsync();

        var messageSession = host.Services.GetRequiredService<IMessageSession>();

        Console.WriteLine("Press 'S' to send a StartOrder message to the server endpoint");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderId = Guid.NewGuid();
            StartOrder startOrder = new()
            {
                OrderId = orderId
            };
            if (key.Key == ConsoleKey.S)
            {
                await messageSession.Send("Samples.CosmosDB.Simple.Server", startOrder);
                Console.WriteLine($"StartOrder Message sent to Server with OrderId {orderId}");
                continue;
            }
            break;
        }

        await host.StopAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             Console.Title = "Client";
         }).UseNServiceBus(x =>
         {
             var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Simple.Client");
             endpointConfiguration.UsePersistence<LearningPersistence>();
             endpointConfiguration.UseTransport(new LearningTransport());
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();

             return endpointConfiguration;
         });
}