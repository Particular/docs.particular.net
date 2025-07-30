using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "RegularEndpoint";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("RegularEndpoint");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<SqsTransport>();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

// Get the required services
var messageSession = host.Services.GetRequiredService<IMessageSession>();
// Register a cancellation token to gracefully handle application shutdown
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press [ENTER] to send a message to the serverless endpoint queue.");
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

    if (key.Key == ConsoleKey.Enter)
    {
        await messageSession.Send("ServerlessEndpoint", new TriggerMessage());
        Console.WriteLine("Message sent to the serverless endpoint queue.");
    }
}

// Wait for the host to stop gracefully
await host.StopAsync();