using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Sender";

// Configure the NServiceBus endpoint
var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Sender");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

// Get the required services
var messageSession = host.Services.GetRequiredService<IMessageSession>();
// Register a cancellation token to gracefully handle application shutdown
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press A or B to publish a message (A and B are tenant IDs). Press Ctrl+C to shut down.");

// Wait for user input to publish messages
while (!ct.IsCancellationRequested)
{
    if (!Console.KeyAvailable)
    {
        // If no key is pressed, wait for a short time before checking again
        await Task.Delay(100, CancellationToken.None);
        continue;
    }

    var input = Console.ReadKey();
    Console.WriteLine();

    var inputKey = char.ToUpperInvariant(input.KeyChar);
    if (inputKey is 'A' or 'B')
    {
        // Send a message to the specified tenant
        var message = new OrderSubmitted
        {
            OrderId = GenerateOrderId(),
            Value = GenerateOrderValue()
        };

        var options = new PublishOptions();
        options.SetHeader("tenant_id", inputKey.ToString());

        await messageSession.Publish(message, options);
    }
    else
    {
        Console.WriteLine($"[{inputKey}] is not a valid tenant identifier.");
    }
}

// Wait for the host to stop gracefully
await host.StopAsync();

static string GenerateOrderId() => Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();

static int GenerateOrderValue(int max = 100) => Random.Shared.Next(max);