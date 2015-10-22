using NServiceBus.Saga;

public class OrderSagaData : ContainSagaData
{
    [Unique]
    public int OrderId { get; set; }
    public int NumberOfItems { get; set; }
}