using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Client";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Transactions.Client");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

// Get the required services
var messageSession = host.Services.GetRequiredService<IMessageSession>();
// Register a cancellation token to gracefully handle application shutdown
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press 'S' to send a StartOrder message to the server endpoint.");
Console.WriteLine("Press Ctrl+C to shut down.");

// Wait for user input to publish messages
while (!ct.IsCancellationRequested)
{
    if (!Console.KeyAvailable)
    {
        // If no key is pressed, wait for a short time before checking again
        await Task.Delay(100, CancellationToken.None);
        continue;
    }

    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.S)
    {
        var orderId = Guid.NewGuid();
        var startOrder = new StartOrder
        {
            OrderId = orderId
        };

        await messageSession.Send("Samples.DynamoDB.Transactions.Server", startOrder);
        Console.WriteLine($"StartOrder Message sent to Server with OrderId {orderId}");
    }
}

// Wait for the host to stop gracefully
await host.StopAsync();
