using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Billing
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Billing";

            var endpointConfiguration = new EndpointConfiguration("Billing");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var simulationEffects = new SimulationEffects();
            endpointConfiguration.RegisterComponents(cc => cc.RegisterSingleton(simulationEffects));

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            RunUserInterfaceLoop(simulationEffects);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        static void RunUserInterfaceLoop(SimulationEffects state)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Billing Endpoint");
                Console.WriteLine("Press N to toggle network latency simulation");
                Console.WriteLine("Press ESC to quit");
                Console.WriteLine();

                state.WriteState(Console.Out);

                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.N:
                        state.ToggleNetworkLatencySimulation();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}