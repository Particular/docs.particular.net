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
            // Choose JSON to serialize and deserialize messages
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            await RunLoop(endpointInstance);

            await endpointInstance.Stop();
        }

        #region RunLoop

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = factory.CreateLogger("Program");

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

                        // Send the command to the local endpoint
                        logger.LogInformation($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                        await endpointInstance.SendLocal(command);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        logger.LogInformation("Unknown input. Please try again.");
                        break;
                }
            }
        }

        #endregion
    }
}