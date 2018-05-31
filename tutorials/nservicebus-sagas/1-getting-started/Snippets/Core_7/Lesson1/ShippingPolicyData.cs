namespace Core_7.Lesson1
{
    using NServiceBus;
    #region BasicShippingPolicyData
    public class ShippingPolicyData : ContainSagaData
    {
        public bool IsOrderPlaced { get; set; }
        public bool IsOrderBilled { get; set; }
    }
    #endregion
}
