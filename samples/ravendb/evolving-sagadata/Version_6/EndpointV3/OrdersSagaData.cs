using NServiceBus;

#region dataV3
public class OrdersSagaData : ContainSagaData
{
    public int OrderId { get; set; }
    public int NumberOfItems { get; set; }
}
#endregion