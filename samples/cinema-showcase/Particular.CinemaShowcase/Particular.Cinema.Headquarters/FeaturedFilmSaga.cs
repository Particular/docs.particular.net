using Microsoft.Extensions.Logging;
using Particular.Cinema.Messages;

namespace Particular.Cinema.Headquarters
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
            // https://docs.particular.net/nservicebus/sagas/message-correlation
            mapper.MapSaga(saga => saga.MonthId)
                .ToMessage<RecordTicketSale>(message => message.MonthId);
        }

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

            if (featuredFilmAfterSale != string.Empty && featuredFilmBeforeNewSale != featuredFilmAfterSale)
            {
                await context.Publish(new FeaturedFilmChanged() { FeaturedFilmName = featuredFilmAfterSale }).ConfigureAwait(false);
            }

            log.LogInformation($"Barbie Tickets: {Data.BarbieTicketCount} \n Oppenheimer Tickets: {Data.OppenheimerTicketCount}");
        }

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
