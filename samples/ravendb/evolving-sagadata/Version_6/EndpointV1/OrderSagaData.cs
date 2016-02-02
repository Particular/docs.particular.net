using NServiceBus;

#region dataV1
public class OrderSagaData : ContainSagaData
{
    public int OrderId { get; set; }
    public int ItemCount { get; set; }
}
#endregion