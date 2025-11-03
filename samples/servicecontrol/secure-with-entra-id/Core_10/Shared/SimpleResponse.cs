using NServiceBus;

public class SimpleResponse :
    IMessage
{
    public string Text { get; init; }
}