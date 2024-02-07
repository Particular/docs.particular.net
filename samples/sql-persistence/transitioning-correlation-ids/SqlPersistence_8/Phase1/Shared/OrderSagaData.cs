using NServiceBus;

#region sagadataPhase1

public class OrderSagaData :
    ContainSagaData
{
    public int OrderNumber { get; set; }
}
#endregion