using NServiceBus;

public class OrderLifecycleSagaData :
    ContainSagaData
{
    public string OrderId { get; set; }
}
