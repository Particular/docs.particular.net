using NServiceBus;

public class DummyMessage :
    IMessage
{
    public string SomeId { get; set; }
}