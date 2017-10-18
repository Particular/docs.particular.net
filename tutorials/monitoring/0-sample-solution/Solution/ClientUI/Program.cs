using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace ClientUI
{
    using System.Threading;

    class Program
    {
        static async Task Main()
        {
            Console.Title = "ClientUI";

            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            var simulatedCustomers = new SimulatedCustomers(endpointInstance);
            var cancellation = new CancellationTokenSource();
            var simulatedWork = simulatedCustomers.Run(cancellation.Token);

            RunUserInterfaceLoop(simulatedCustomers);

            cancellation.Cancel();

            await simulatedWork
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        static void RunUserInterfaceLoop(SimulatedCustomers simulatedCustomers)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Simulating customers placing orders on a website");
                Console.WriteLine("Press T to toggle High/Low traffic mode");
                Console.WriteLine("Press ESC to quit");
                Console.WriteLine();

                simulatedCustomers.WriteState(Console.Out);

                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.T:
                        simulatedCustomers.ToggleTrafficMode();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}