using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Client UI";

var builder = Host.CreateApplicationBuilder();

builder.AddServiceDefaults();
builder.AddNServiceBusEndpoint("ClientUI", (_, routing) =>
{
    routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
});


var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Sending messages on an increasing interval. Press [CTRL] + [C] to exit");

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
        var i = 1;

        while (true)
        {
            await messageSession.Send(new PlaceOrder { OrderId = Guid.NewGuid().ToString() }, cts.Token);
            Console.WriteLine("Sent a message");

            await Task.Delay(i++ * 500, cts.Token);
        }
    }
    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
    {
        // graceful shutdown
    }
}

await host.StopAsync();