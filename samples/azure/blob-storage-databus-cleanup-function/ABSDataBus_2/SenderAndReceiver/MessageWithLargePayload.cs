using NServiceBus;

public class MessageWithLargePayload :
    ICommand
{
    public DataBusProperty<byte[]> LargePayload { get; set; }
    public string Description { get; set; }
}