using Microsoft.Extensions.Hosting;
using Particular.Cinema.Messages;

namespace Particular.Cinema.TicketSales
{
    class Worker : BackgroundService
    {
        private readonly IMessageSession messageSession;
        private static string MonthId = DateTime.Now.ToString("yyyy-MM");

        public Worker(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("To enter number of tickets sold for Barbie press b");
                Console.WriteLine("To enter number of tickets sold for Oppenheimer press o");
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!Console.KeyAvailable) continue;

                    var userInput = Console.ReadKey();
                    switch (userInput.Key)
                    {
                        case ConsoleKey.B:
                            await messageSession.Send(new RecordTicketSale  { MonthId = MonthId, FilmName = "Barbie" }, cancellationToken)
                        .ConfigureAwait(false);
                            break;
                        case ConsoleKey.O:
                            await messageSession.Send(new RecordTicketSale { MonthId = MonthId, FilmName = "Oppenheimer" }, cancellationToken)
                        .ConfigureAwait(false);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }
        }
    }
}
