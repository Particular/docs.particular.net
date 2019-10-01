using NServiceBus;

class MyMessage :
    IMessage
{
    public string Contents { get; set; }
}