using System;
using System.Threading;
using System.Threading.Tasks;
using Core_9.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9
{

    #region PlaceOrder

    namespace Messages
    {
        public class PlaceOrder :
            ICommand
        {
            public string OrderId { get; set; }
        }
    }

    #endregion

    class Program
    {
        static async Task Main(EndpointConfiguration endpointConfiguration)
        {
            #region AddRunLoopToMain

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            // Remove these two lines
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            // Replace with:
            await RunLoop(endpointInstance);

            await endpointInstance.Stop();

            #endregion
        }

        static async Task Steps(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            
            #region AddWorker
            builder.UseNServiceBus(endpointConfiguration);

            // Remove these two lines
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            // Replace with:
            builder.Services.AddHostedService<Worker>();

            await builder.Build().RunAsync();
            #endregion
        }

        static Task RunLoop(IEndpointInstance endpoint)
        {
            return Task.CompletedTask;
        }
    }

    #region WorkerClass

    public class Worker(IMessageSession messageSession, ILogger<Worker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Console.Title = "ClientUI";

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
                        await messageSession.SendLocal(command);

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

    #endregion
}