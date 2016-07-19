using NServiceBus;

public class EnumMessage :
    IMessage
{
    public string Property { get; set; }
}