using NServiceBus.Saga;

public class OrdersSagaData : ContainSagaData
{
    [Unique]
    public int OrderId { get; set; }
    public int NumberOfItems { get; set; }
}