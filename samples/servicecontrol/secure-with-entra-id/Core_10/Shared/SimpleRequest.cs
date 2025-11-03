using NServiceBus;

public class SimpleRequest :
    IMessage
{
    public string Text { get; init; }
}