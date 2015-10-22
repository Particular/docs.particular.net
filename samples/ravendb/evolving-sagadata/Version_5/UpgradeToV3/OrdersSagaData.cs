using NServiceBus.Saga;

public class OrdersSagaData : ContainSagaData
{
    public int OrderId { get; set; }
    public int NumberOfItems { get; set; }
}
