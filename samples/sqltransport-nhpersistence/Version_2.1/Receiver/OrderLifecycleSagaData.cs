using NServiceBus.Saga;

public class OrderLifecycleSagaData : ContainSagaData
{
    public virtual string OrderId { get; set; }
}