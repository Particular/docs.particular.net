namespace VideoStore.Messages.Events
{
    public interface OrderPlaced
    {
        int OrderNumber { get; set; }
        string[] ProductIds { get; set; }
        string ClientId { get; set; }
    }
}