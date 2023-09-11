using Microsoft.Extensions.Logging;
using Particular.Cinema.Messages;

namespace Particular.Cinema.TicketSales
{
    public class FeaturedFilmChangedHandler : IHandleMessages<FeaturedFilmChanged>
    {
        private readonly ILogger log;

        public FeaturedFilmChangedHandler(ILogger<FeaturedFilmChangedHandler> log)
        {
            this.log = log;
        }

        public Task Handle(FeaturedFilmChanged message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Featured Film of the month is......{message.FeaturedFilmName}!!");
            return Task.CompletedTask;
        }
    }
}
