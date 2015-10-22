using NServiceBus.Saga;

public class OrderSagaData : ContainSagaData
{
    public int OrderId { get; set; }
    public int ItemCount { get; set; }
}
