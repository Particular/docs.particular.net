using NServiceBus;

public class ObjectMessage :
    IMessage
{
    public string Property { get; set; }
}