using NServiceBus.Saga;

#region dataV2
public class OrderSagaData : ContainSagaData
{
    [Unique]
    public int OrderId { get; set; }
    public int NumberOfItems { get; set; }
}
#endregion