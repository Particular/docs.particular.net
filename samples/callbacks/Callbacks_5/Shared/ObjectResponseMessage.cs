using NServiceBus;

public class ObjectResponseMessage :
    IMessage
{
    public string Property { get; set; }
}
