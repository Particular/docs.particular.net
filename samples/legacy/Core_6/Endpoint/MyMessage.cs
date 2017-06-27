using NServiceBus;

class MyMessage : IMessage
{
    public decimal Value { get; set; }
}