using NServiceBus.Saga;

#region dataV3
public class OrdersSagaData : ContainSagaData
{
    [Unique]
    public int OrderId { get; set; }
    public int NumberOfItems { get; set; }
}
#endregion