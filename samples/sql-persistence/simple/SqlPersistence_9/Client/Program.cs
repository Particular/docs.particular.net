using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SharedMessages;

Console.Title = "Client";

var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.Client");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

// Set up the generic host and register NServiceBus
var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

// Configure logging
builder.Logging.AddConsole();

// Build and start the host
var host = builder.Build();
await host.StartAsync();

// Get the required services from the host
var messageSession = host.Services.GetRequiredService<IMessageSession>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

// Get the application stopping cancellation token to handle graceful shutdown
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press [S] to send a StartOrder message to the SqlServer endpoint");
Console.WriteLine("Press [M] to send a StartOrder message to the MySql endpoint");
Console.WriteLine("Press [O] to send a StartOrder message to the Oracle endpoint");
Console.WriteLine("Press [P] to send a StartOrder message to the PostgreSql endpoint");
Console.WriteLine("Press [CTRL+C] to exit");

while (!ct.IsCancellationRequested)
{
    if (!Console.KeyAvailable)
    {
        // Wait a short time before checking again
        await Task.Delay(100, CancellationToken.None);
        continue;
    }

    var key = Console.ReadKey();
    Console.WriteLine();

    // Create the order message
    var startOrder = new StartOrder
    {
        OrderId = Guid.NewGuid()
    };

    if (key.Key == ConsoleKey.M)
    {
        await messageSession.Send("Samples.SqlPersistence.EndpointMySql", startOrder);
        logger.LogInformation("StartOrder Message sent to EndpointMySql with OrderId {OrderId}", startOrder.OrderId);
        continue;
    }
    if (key.Key == ConsoleKey.S)
    {
        await messageSession.Send("Samples.SqlPersistence.EndpointSqlServer", startOrder);
        logger.LogInformation("StartOrder Message sent to EndpointSqlServer with OrderId {orderId}", startOrder.OrderId);
        continue;
    }
    if (key.Key == ConsoleKey.O)
    {
        await messageSession.Send("EndpointOracle", startOrder);
        logger.LogInformation("StartOrder Message sent to EndpointOracle with OrderId {orderId}", startOrder.OrderId);
        continue;
    }
    if (key.Key == ConsoleKey.P)
    {
        await messageSession.Send("EndpointPostgreSql", startOrder);
        logger.LogInformation("StartOrder Message sent to EndpointPostgreSql with OrderId {orderId}", startOrder.OrderId);
        continue;
    }
    break;
}

// Ensure the host is stopped gracefully
logger.LogInformation("Stopping host...");
await host.StopAsync();
logger.LogInformation("Host stopped successfully");