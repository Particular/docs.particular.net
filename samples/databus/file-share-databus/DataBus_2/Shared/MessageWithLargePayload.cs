using NServiceBus;

#region MessageWithLargePayload

//the claim check is allowed to clean up transmitted properties older than the TTBR
[TimeToBeReceived("00:01:00")]
public class MessageWithLargePayload :
    ICommand
{
    public string SomeProperty { get; set; }
    public ClaimCheckProperty<byte[]> LargeBlob { get; set; }
}

#endregion