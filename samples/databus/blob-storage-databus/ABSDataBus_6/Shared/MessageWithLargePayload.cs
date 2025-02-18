using NServiceBus;
using NServiceBus.ClaimCheck;

#region MessageWithLargePayload

[TimeToBeReceived("00:03:00")]
public class MessageWithLargePayload :
    ICommand
{
    public ClaimCheckProperty<byte[]> LargePayload { get; set; }
    public string Description { get; set; }
}

#endregion
