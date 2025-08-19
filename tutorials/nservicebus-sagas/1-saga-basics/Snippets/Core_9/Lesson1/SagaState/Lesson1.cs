using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.Lesson1.SagaState
{
#pragma warning disable 1998

    #region ShippingPolicyAugmentedWithData
    public class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicyData>,
        IHandleMessages<OrderPlaced>,
        IHandleMessages<OrderBilled>
    #endregion
    {
       
        #region EmptyConfigureHowToFindSaga
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            // TODO
        }
        #endregion

        #region HandleBasicImplementation
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
            Data.IsOrderPlaced = true;
            return Task.CompletedTask;
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
            Data.IsOrderBilled = true;
            return Task.CompletedTask;
        }
        #endregion
    }

#pragma warning restore 1998
}
