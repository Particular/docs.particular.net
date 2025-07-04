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

        #region featured-film-changed-handler
        public Task Handle(FeaturedFilmChanged message, IMessageHandlerContext context)
        {
            log.LogInformation("Featured Film of the month is....{FeaturedFilmName}!!", message.FeaturedFilmName);
            return Task.CompletedTask;
        }
        #endregion
    }
}
