using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

#region AddRunLoop
builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.StartAsync();

var messageSession = app.Services.GetRequiredService<IMessageSession>();
await RunLoop(messageSession);

await app.StopAsync();
#endregion

#region RunLoop

static async Task RunLoop(IMessageSession messageSession)
{
    while (true)
    {
        Console.WriteLine("Press 'P' to place an order, or 'Q' to quit.");
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
                Console.WriteLine($"PlaceOrder sent, OrderId = {command.OrderId}");
                await messageSession.SendLocal(command);

                break;

            case ConsoleKey.Q:
                return;

            default:
                Console.WriteLine("Unknown input. Please try again.");
                break;
        }
    }
}

#endregion