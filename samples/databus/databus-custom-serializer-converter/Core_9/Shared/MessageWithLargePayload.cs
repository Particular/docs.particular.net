using NServiceBus;

#pragma warning disable CS0618 // Type or member is obsolete
#region MessageWithLargePayload

//the data bus is allowed to clean up transmitted properties older than the TTBR
[TimeToBeReceived("00:01:00")]
public class MessageWithLargePayload :
    ICommand
{
    public string SomeProperty { get; set; }
    public DataBusProperty<byte[]> LargeBlob { get; set; }
}

#endregion
#pragma warning restore CS0618 // Type or member is obsolete