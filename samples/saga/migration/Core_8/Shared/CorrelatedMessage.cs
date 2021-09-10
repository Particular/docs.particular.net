using NServiceBus;

public class CorrelatedMessage :
    IMessage
{
    public string SomeId { get; set; }
}