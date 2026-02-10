using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClientUI;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("ClientUI");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var routing = endpointConfiguration.UseTransport(new LearningTransport());

        routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
        routing.RouteToEndpoint(typeof(CancelOrder), "Sales");

        builder.UseNServiceBus(endpointConfiguration);

        var host = builder.Build();
        await host.StartAsync();

        var messageSession = host.Services.GetRequiredService<IMessageSession>();

        while (true)
        {
            Console.WriteLine("Press 'P' to place an order, or 'Q' to quit.");
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.Q)
            {
                break;
            }

            if (key.Key == ConsoleKey.P)
            {
                // Instantiate the command
                var command = new PlaceOrder
                {
                    OrderId = Guid.NewGuid().ToString()
                };

                // Send the command
                Console.WriteLine($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                await messageSession.Send(command);
            }

            else
            {
                Console.WriteLine("Unknown input. Please try again.");
            }
        }

        await host.StopAsync();

    }
}
