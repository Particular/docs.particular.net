namespace Core_7.ReplyToOriginator
{
    using NServiceBus;
    using System.Threading.Tasks;

    class ReplyToOriginator :
             Saga<ShippingPolicyData>,
             IAmStartedByMessages<OrderBilled>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.ConfigureMapping<OrderBilled>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Data.IsOrderBilled = true;
            #region ReplyToOriginator
            await ReplyToOriginator(context, new PackageSuccessfullyShipped())
                .ConfigureAwait(false);
            #endregion
        }

    }
    
    internal class ShippingPolicyData :
        ContainSagaData
    {
        public string OrderId { get; set; }
        public bool IsOrderPlaced { get; set; }
        public bool IsOrderBilled { get; set; }
    }

    internal class PackageSuccessfullyShipped
    {
        public PackageSuccessfullyShipped()
        {
        }
    }

    class OrderBilled
    {
        public object OrderId { get; internal set; }
    }
}