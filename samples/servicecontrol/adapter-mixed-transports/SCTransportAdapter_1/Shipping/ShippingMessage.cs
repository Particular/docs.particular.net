using NServiceBus;

class ShippingMessage :
    IMessage
{
    public string Id { get; set; }
}