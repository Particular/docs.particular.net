using Microsoft.Extensions.Logging;
using Cinema.Messages;

namespace Cinema.Headquarters
{
    class FeaturedFilmSaga : Saga<FeaturedFilmSagaData>,
        IAmStartedByMessages<RecordTicketSale>
    {
        private readonly ILogger log;

        public FeaturedFilmSaga(ILogger<FeaturedFilmSaga> log)
        {
            this.log = log;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<FeaturedFilmSagaData> mapper)
        {
            // https://docs.net/nservicebus/sagas/message-correlation
            // There will be one saga instance per month
            mapper.MapSaga(saga => saga.MonthId)
                .ToMessage<RecordTicketSale>(message => message.MonthId);
        }

        #region ticket-sales-handler
        public async Task Handle(RecordTicketSale message, IMessageHandlerContext context)
        {
            string featuredFilmBeforeNewSale = GetFeaturedFilm();

            switch (message.FilmName)
            {
                case "Barbie":
                    Data.BarbieTicketCount++;
                    break;
                case "Oppenheimer":
                    Data.OppenheimerTicketCount++;
                    break;
                default:
                    break;
            }

            string featuredFilmAfterSale = GetFeaturedFilm();

            if (featuredFilmAfterSale != string.Empty
                && featuredFilmBeforeNewSale != featuredFilmAfterSale)
            {
                log.LogInformation("Featured film changed: {FeaturedFilm}", featuredFilmAfterSale);
                await context.Publish(new FeaturedFilmChanged
                {
                    FeaturedFilmName = featuredFilmAfterSale
                });
            }

            log.LogInformation("Barbie Tickets: {BarbieCount}\nOppenheimer Tickets: {OppenheimerCount}", Data.BarbieTicketCount, Data.OppenheimerTicketCount);
        }
        #endregion

        string GetFeaturedFilm()
        {
            if (Data.BarbieTicketCount == Data.OppenheimerTicketCount) return string.Empty;

            if (Data.BarbieTicketCount > Data.OppenheimerTicketCount) return "Barbie";

            return "Oppenheimer";
        }
    }

    class FeaturedFilmSagaData : ContainSagaData
    {
        public string? MonthId { get; set; }

        public int BarbieTicketCount { get; set; }
        public int OppenheimerTicketCount { get; set; }
    }
}
