using Microsoft.Extensions.Logging;

namespace Lesson1.StartingTheSaga
{

#pragma warning disable NSB0006 // Message that starts the saga does not have a message mapping

    namespace StartedBy1Message
    {
        #region ShippingPolicyStartedBy1Message
        public class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicyData>,
            IAmStartedByMessages<OrderPlaced>, // This can start the saga
            IHandleMessages<OrderBilled>       // But surely, not this one!?
        #endregion
        {

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
            {
                // TODO
            }

            public Task Handle(OrderPlaced message, IMessageHandlerContext context)
            {
                logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
                return Task.CompletedTask;
            }

            public Task Handle(OrderBilled message, IMessageHandlerContext context)
            {
                logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
                return Task.CompletedTask;
            }
        }
    }

    namespace StartedBy2Messages
    {
        #region ShippingPolicyStartedBy2Messages
        public class ShippingPolicy(ILogger<ShippingPolicy> logger) : Saga<ShippingPolicyData>,
            IAmStartedByMessages<OrderPlaced>, // I can start the saga!
            IAmStartedByMessages<OrderBilled>  // I can start the saga too!
        {
            #endregion

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
            {
                // TODO
            }

            public Task Handle(OrderPlaced message, IMessageHandlerContext context)
            {
                logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
                return Task.CompletedTask;
            }

            public Task Handle(OrderBilled message, IMessageHandlerContext context)
            {
                logger.LogInformation("OrderPlaced message received for {OrderId}.", message.OrderId);
                return Task.CompletedTask;
            }
        }
    }

#pragma warning restore NSB0006 // Message that starts the saga does not have a message mapping
}