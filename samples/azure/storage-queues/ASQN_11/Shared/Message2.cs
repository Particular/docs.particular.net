using NServiceBus;

public class MyResponse :
    IMessage
{
    public string Property { get; set; }
}