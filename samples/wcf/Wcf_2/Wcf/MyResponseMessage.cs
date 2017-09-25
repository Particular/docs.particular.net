using NServiceBus;

public class MyResponseMessage :
    IMessage
{
    public string Info { get; set; }
}