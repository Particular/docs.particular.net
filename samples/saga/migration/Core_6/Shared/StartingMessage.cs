using NServiceBus;

public class StartingMessage :
    IMessage
{
    public string SomeId { get; set; }
}