using NServiceBus;

#pragma warning disable CS0618 // Type or member is obsolete
#region MessageWithLargePayload

[TimeToBeReceived("00:03:00")]
public class MessageWithLargePayload :
    ICommand
{
    public DataBusProperty<byte[]> LargePayload { get; set; }
    public string Description { get; set; }
}

#endregion
#pragma warning restore CS0618 // Type or member is obsolete