using Microsoft.Extensions.Logging;
using Cinema.Messages;

namespace Cinema.TicketSales
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
            log.LogInformation($"Featured Film of the month is......{message.FeaturedFilmName}!!");
            return Task.CompletedTask;
        }
    }
}
