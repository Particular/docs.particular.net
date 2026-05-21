using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "ClientUI";

var builder = Host.CreateApplicationBuilder(args);

builder.AddNServiceBusEndpoint("ClientUI", (_, routing) =>
{
    routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
});

var app = builder.Build();

await app.StartAsync();

var messageSession = app.Services.GetRequiredService<IMessageSession>();
await RunLoop(messageSession);

await app.StopAsync();

static async Task RunLoop(IMessageSession messageSession)
{
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

                await Task.Delay(i++ * 50, cts.Token);
            }
        }
        catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
        {
            // graceful shutdown
        }
    }
}