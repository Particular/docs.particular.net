using NServiceBus;

#region dataV2

public class OrderSagaData : ContainSagaData
{
    public int OrderId { get; set; }
    public int NumberOfItems { get; set; }
}
#endregion