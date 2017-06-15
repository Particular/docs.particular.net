using NServiceBus;

class SalesMessage :
    IMessage
{
    public string Id { get; set; }
}