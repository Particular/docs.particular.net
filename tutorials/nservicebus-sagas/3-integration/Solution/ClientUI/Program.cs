using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "ClientUI";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport(new LearningTransport());
routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
routing.RouteToEndpoint(typeof(CancelOrder), "Sales");

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.StartAsync();

var messageSession = app.Services.GetRequiredService<IMessageSession>();
await RunLoop(messageSession);

await app.StopAsync();

static async Task RunLoop(IMessageSession messageSession)
{
    var lastOrder = string.Empty;

    while (true)
    {
        Console.WriteLine("Press 'P' to place an order, 'C' to cancel an order, or 'Q' to quit.");
        var key = Console.ReadKey();
        Console.WriteLine();

        switch (key.Key)
        {
            case ConsoleKey.P:
                // Instantiate the command
                var command = new PlaceOrder
                {
                    OrderId = Guid.NewGuid().ToString()
                };

                // Send the command
                Console.WriteLine($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                await messageSession.Send(command);

                lastOrder = command.OrderId; // Store order identifier to cancel if needed.
                break;

            case ConsoleKey.C:
                var cancelCommand = new CancelOrder
                {
                    OrderId = lastOrder
                };
                await messageSession.Send(cancelCommand);
                Console.WriteLine($"Sent a CancelOrder command, {cancelCommand.OrderId}");
                break;

            case ConsoleKey.Q:
                return;

            default:
                Console.WriteLine("Unknown input. Please try again.");
                break;
        }
    }
}