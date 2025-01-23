namespace ClientUI
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    class ConsoleBackgroundService(SimulatedCustomers simulatedCustomers) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Console.Clear();
                await Console.Out.WriteLineAsync("""
                    Simulating customers placing orders on a website
                    Press CTRL+C to quit

                    """);

                simulatedCustomers.WriteState(Console.Out);

                while (!Console.KeyAvailable)
                {
                    await Task.Delay(15, stoppingToken);
                }

                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.I:
                        simulatedCustomers.IncreaseTraffic();
                        break;
                    case ConsoleKey.D:
                        simulatedCustomers.DecreaseTraffic();
                        break;
                }
            }
        }
    }
}
