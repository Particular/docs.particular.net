using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using Microsoft.Extensions.Logging;

namespace ClientUI
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "ClientUI";

            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            await RunLoop(endpointInstance);

            await endpointInstance.Stop();
        }

        private static readonly ILogger<Program> logger =
        LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<Program>();

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                logger.LogInformation("Press 'P' to place an order, or 'Q' to quit.");
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
                        logger.LogInformation("Sending PlaceOrder command, OrderId = {OrderId}", command.OrderId);
                        await endpointInstance.Send(command);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        logger.LogInformation("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}