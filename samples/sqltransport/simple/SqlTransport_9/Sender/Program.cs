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

var host = builder.Build();

await host.StartAsync();

// Get the required services from the host
var messageSession = host.Services.GetRequiredService<IMessageSession>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

// Get the application stopping cancellation token to handle graceful shutdown
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press [c] to send a command, or [e] to publish an event. Press CTRL+C to exit.");
while (!ct.IsCancellationRequested)
{
    if (!Console.KeyAvailable)
    {
        // Wait a short time before checking again
        await Task.Delay(100, CancellationToken.None);
        continue;
    }

    var input = Console.ReadKey();
    Console.WriteLine();

    switch (input.Key)
    {
        case ConsoleKey.C:
            // Send a command message to the configured receiver endpoint
            logger.LogInformation("Sending command...");
            await messageSession.Send(new MyCommand(), ct);
            logger.LogInformation("Command sent successfully");
            break;
        case ConsoleKey.E:
            // Publish an event message to all interested subscribers
            logger.LogInformation("Publishing event...");
            await messageSession.Publish(new MyEvent(), ct);
            logger.LogInformation("Event published successfully");
            break;
    }
}

// Ensure the host is stopped gracefully
logger.LogInformation("Stopping host...");
await host.StopAsync();
logger.LogInformation("Host stopped successfully");