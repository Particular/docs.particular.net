using Shared;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Sender");

var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString") ?? string.Empty;
var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default));
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.AuditProcessedMessagesTo("audit");
routing.RouteToEndpoint(typeof(Ping), "Receiver");

// Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Publishing messages... Press [Ctrl] + [C] to cancel");

using (var cts = new CancellationTokenSource())
{
    Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("Cancellation Requested...");
        cts.Cancel();
        e.Cancel = true;
    };

    try
    {
        var number = 0;

        while (true)
        {
            await messageSession.Send(new Ping { Round = number++ });

            Console.WriteLine($"Sent message #{number}");

            await Task.Delay(1_000, cts.Token);
        }
    }
    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
    {
        // graceful shutdown
    }
}

await host.StopAsync();