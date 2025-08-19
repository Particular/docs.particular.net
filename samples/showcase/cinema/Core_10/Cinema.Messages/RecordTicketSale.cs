namespace Cinema.Messages
{
    public class RecordTicketSale : IMessage
    {
        public string? MonthId { get; set; }
        public string? FilmName { get; set; }
    }
}