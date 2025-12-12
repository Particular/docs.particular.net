using NServiceBus;

public class MyMessage : IMessage
{
    public required string CorrelationID { get; set; }
    public required int SequenceNumber { get; set; }
}