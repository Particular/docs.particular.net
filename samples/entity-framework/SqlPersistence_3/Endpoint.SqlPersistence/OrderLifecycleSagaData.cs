using NServiceBus;

public class OrderLifecycleSagaData :
    ContainSagaData
{
    public virtual string OrderId { get; set; }
}
