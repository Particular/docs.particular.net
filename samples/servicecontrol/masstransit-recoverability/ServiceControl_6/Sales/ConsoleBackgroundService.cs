namespace Sales
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    class ConsoleBackgroundService(SimulationEffects state) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Console.Clear();
                await Console.Out.WriteAsync("""
                Sales Endpoint
                Press I to increase the simulated failure rate
                Press D to decrease the simulated failure rate
                Press R to reset simulation
                Press CTRL+C to quit

                """);

                state.WriteState(Console.Out);

                while (!Console.KeyAvailable)
                {
                    await Task.Delay(15, stoppingToken);
                }

                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.I:
                        state.IncreaseFailureRate();
                        break;
                    case ConsoleKey.D:
                        state.DecreaseFailureRate();
                        break;
                    case ConsoleKey.Q:
                        state.ProcessMessagesFaster();
                        break;
                    case ConsoleKey.S:
                        state.ProcessMessagesSlower();
                        break;
                    case ConsoleKey.R:
                        state.Reset();
                        break;
                }
            }
        }
    }
}
