namespace Messages
{
    using NServiceBus;

    [TimeToBeReceived("00:03:00")]
    public class MessageWithLargePayload : ICommand
    {
        public string Description { get; set; }
        public DataBusProperty<byte[]> LargePayload { get; set; }
    }
}
