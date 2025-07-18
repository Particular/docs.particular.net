using NServiceBus;

[TimeToBeReceived("00:00:10")]
public class MessageWithLargePayload : ICommand
{
#pragma warning disable CS0618 // Type or member is obsolete
    public DataBusProperty<byte[]> LargePayload { get; set; }
#pragma warning restore CS0618 // Type or member is obsolete
    public string Description { get; set; }
}