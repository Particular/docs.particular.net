using Cinema.Messages;
using Microsoft.Extensions.Logging;

namespace Cinema.TicketSales
{
    public class FeaturedFilmChangedHandler(ILogger<FeaturedFilmChangedHandler> log) : IHandleMessages<FeaturedFilmChanged>
    {
        private readonly ILogger log = log;

        #region featured-film-changed-handler
        public Task Handle(FeaturedFilmChanged message, IMessageHandlerContext context)
        {
            log.LogInformation("Featured Film of the month is....{FeaturedFilmName}!!", message.FeaturedFilmName);
            return Task.CompletedTask;
        }
        #endregion
    }
}
