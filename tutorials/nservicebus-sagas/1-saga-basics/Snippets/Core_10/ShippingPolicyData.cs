namespace Lesson1;

#region BasicShippingPolicyData

public class ShippingPolicyData : ContainSagaData
{
    public bool IsOrderPlaced { get; set; }
    public bool IsOrderBilled { get; set; }
}
#endregion