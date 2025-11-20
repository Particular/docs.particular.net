using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "SimpleSender";

var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleSender");
endpointConfiguration.EnableInstallers();

#region TransportConfiguration
// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SqlServerSimple;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=SqlServerSimple;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
var routing = endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

routing.RouteToEndpoint(typeof(MyCommand), "Samples.SqlServer.SimpleReceiver");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

await SqlHelper.EnsureDatabaseExists(connectionString);

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

// Get the required services from the host
var messageSession = host.Services.GetRequiredService<IMessageSession>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

Console.WriteLine("Press [c] to send a command, or [e] to publish an event. Press any other key to exit.");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.C && key.Key != ConsoleKey.E)
    {
        break;
    }

    if (key.Key == ConsoleKey.C)
    {
        // Send a command message to the configured receiver endpoint
        logger.LogInformation("Sending command...");
        await messageSession.Send(new MyCommand());
        logger.LogInformation("Command sent successfully");
    }
    else if (key.Key == ConsoleKey.E)
    {
        // Publish an event message to all interested subscribers
        logger.LogInformation("Publishing event...");
        await messageSession.Publish(new MyEvent());
        logger.LogInformation("Event published successfully");
    }
}

// Ensure the host is stopped gracefully
logger.LogInformation("Stopping host...");
await host.StopAsync();
logger.LogInformation("Host stopped successfully");
