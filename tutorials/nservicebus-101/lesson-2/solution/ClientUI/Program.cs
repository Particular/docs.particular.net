using System;
using System.Threading.Tasks;
using Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;

namespace ClientUI
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "ClientUI";

            var endpointConfig = new EndpointConfiguration("ClientUI");
            endpointConfig.UseTransport<MsmqTransport>();
            endpointConfig.UseSerialization<JsonSerializer>();
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.SendFailedMessagesTo("error");
            endpointConfig.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfig).ConfigureAwait(false);

            await RunLoop(endpointInstance);

            await endpointInstance.Stop().ConfigureAwait(false);
        }

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            var logger = LogManager.GetLogger<Program>();

            while (true)
            {
                logger.Info("Enter 'placeorder' to place an order, or 'quit' to quit.");
                string input = Console.ReadLine();

                switch (input?.ToLower())
                {
                    case "placeorder":
                        // Instantiate the command
                        var command = new PlaceOrder { OrderId = Guid.NewGuid().ToString() };

                        // Send the command to the current
                        logger.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                        await endpointInstance.SendLocal(command).ConfigureAwait(false);

                        break;

                    case "quit":
                        return;

                    default:
                        logger.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}
