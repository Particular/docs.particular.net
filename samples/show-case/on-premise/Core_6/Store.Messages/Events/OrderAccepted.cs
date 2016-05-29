namespace Store.Messages.Events
{
    public interface OrderAccepted
    {
        int OrderNumber { get; set; }
        string[] ProductIds { get; set; }
        string ClientId { get; set; }
    }
}