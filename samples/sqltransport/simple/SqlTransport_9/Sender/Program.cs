using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

Console.Title = "SimpleSender";

// Configure the NServiceBus endpoint for the sender
var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleSender");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#region TransportConfiguration
// SQL Server transport configuration with different connection string options
// for SqlExpress:
//var connectionString = @"Data Source=.\SqlExpress;Initial Catalog=SqlServerSimple;Integrated Security=True;Max Pool Size=100;Encrypt=false";
// for SqlServer:
//var connectionString = @"Server=localhost,1433;Initial Catalog=SqlServerSimple;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
// for LocalDB (default choice for development):
var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SqlServerSimple;Integrated Security=True;Connect Timeout=30;Max Pool Size=100;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

// Configure SQL Server transport
var routing = endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

// Define message routing: MyCommand messages should be sent to the SimpleReceiver endpoint
routing.RouteToEndpoint(typeof(MyCommand), "Samples.SqlServer.SimpleReceiver");

#endregion

// Ensure the database exists before starting the endpoint
await SqlHelper.EnsureDatabaseExists(connectionString);

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

// Configure logging to output to console with minimum Information level
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var host = builder.Build();

await host.StartAsync();

// Get the required services from the host
var messageSession = host.Services.GetRequiredService<IMessageSession>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

// Create a cancellation token source for coordinating graceful shutdown
var cts = new CancellationTokenSource();

// Set up the Ctrl+C handler for graceful shutdown
Console.CancelKeyPress += (sender, e) =>
{
    logger.LogInformation("Ctrl+C detected. Shutting down gracefully...");

    // Prevent the process from terminating immediately
    e.Cancel = true;
    cts.Cancel();
};

logger.LogInformation("Press [c] to send a command, or [e] to publish an event. Press CTRL+C to exit.");

try
{

    while (!cts.Token.IsCancellationRequested)
    {
        if (!Console.KeyAvailable)
        {
            // Wait a short time before checking again, respecting cancellation
            await Task.Delay(100, cts.Token);
            continue;
        }

        var input = Console.ReadKey();
        Console.WriteLine();

        switch (input.Key)
        {
            case ConsoleKey.C:
                // Send a command message to the configured receiver endpoint
                logger.LogInformation("Sending command...");
                await messageSession.Send(new MyCommand(), cts.Token);
                logger.LogInformation("Command sent successfully");
                break;
            case ConsoleKey.E:
                // Publish an event message to all interested subscribers
                logger.LogInformation("Publishing event...");
                await messageSession.Publish(new MyEvent(), cts.Token);
                logger.LogInformation("Event published successfully");
                break;
        }
    }
}
catch (TaskCanceledException)
{
    // This exception is expected when cts.Cancel() is called during shutdown.
    // We can safely ignore it and proceed with graceful shutdown.
    logger.LogInformation("Application cancelled gracefully");
}
catch (Exception ex)
{
    // Log any unexpected errors with full exception details
    logger.LogError(ex, "An unexpected error occurred");
    throw; // Re-throw to maintain error handling behavior
}
finally
{
    // Ensure the host is stopped gracefully, regardless of how the try block exits
    logger.LogInformation("Stopping host...");
    await host.StopAsync();
    logger.LogInformation("Host stopped successfully");
}
