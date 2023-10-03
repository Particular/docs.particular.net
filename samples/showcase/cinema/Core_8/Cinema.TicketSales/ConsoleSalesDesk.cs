using Microsoft.Extensions.Hosting;
using Cinema.Messages;

namespace Cinema.TicketSales
{
    class ConsoleSalesDesk : BackgroundService
    {
        private readonly IMessageSession messageSession;
        private static string MonthId = DateTime.Now.ToString("yyyy-MM");

        // IMessageSession is added to the services collection by NServiceBus.
        public ConsoleSalesDesk(IMessageSession messageSession)
        {
            // The message session is used to access NServiceBus functions.
            // In this class it is used to send a RecordTicketSale message.
            this.messageSession = messageSession;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                #region sales-desk
                Console.WriteLine("B) Sell ticket for Barbie");
                Console.WriteLine("O) Sell ticket for Oppenheimer");

                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!Console.KeyAvailable) continue;

                    var userInput = Console.ReadKey();
                    switch (userInput.Key)
                    {
                        case ConsoleKey.B:
                            await messageSession.Send(new RecordTicketSale
                            {
                                MonthId = MonthId,
                                FilmName = "Barbie"
                            }, cancellationToken)
                        .ConfigureAwait(false);
                            break;
                        case ConsoleKey.O:
                            await messageSession.Send(new RecordTicketSale
                            {
                                MonthId = MonthId,
                                FilmName = "Oppenheimer"
                            }, cancellationToken)
                        .ConfigureAwait(false);
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }
        }
    }
}
