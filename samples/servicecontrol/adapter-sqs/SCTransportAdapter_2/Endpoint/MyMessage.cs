using NServiceBus;

class MyMessage :
    IMessage
{
    public string Id { get; set; }
}