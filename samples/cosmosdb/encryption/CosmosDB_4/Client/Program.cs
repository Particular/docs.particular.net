using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Client";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Encryption.Client");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

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
    StartOrder startOrder = new()
    {
        OrderId = orderId
    };
    if (key.Key == ConsoleKey.S)
    {
        await messageSession.Send("Samples.CosmosDB.Encryption.Server", startOrder);
        Console.WriteLine($"StartOrder Message sent to Server with OrderId {orderId}");
        continue;
    }
    break;
}

await host.StopAsync();
