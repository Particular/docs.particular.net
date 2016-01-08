using NServiceBus.Saga;

#region dataV1
public class OrderSagaData : ContainSagaData
{
    [Unique]
    public int OrderId { get; set; }
    public int ItemCount { get; set; }
}
#endregion