using NServiceBus;

[TimeToBeReceived("00:00:10")]
public class MessageWithLargePayload : ICommand
{
    public ClaimCheckProperty<byte[]> LargePayload { get; set; }
    public string Description { get; set; }
}