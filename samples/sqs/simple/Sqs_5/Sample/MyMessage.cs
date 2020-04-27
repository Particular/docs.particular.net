using NServiceBus;

public class MyMessage :
    IMessage
{
    public byte[] Data { get; set; }
}