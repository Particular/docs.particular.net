﻿using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Shipping
{
    class ShippingPolicy : Saga<ShippingPolicyData>,
        IAmStartedByMessages<OrderBilled>,
        IAmStartedByMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.MapSaga(sagaData => sagaData.OrderId)
                .ToMessage<OrderPlaced>(message => message.OrderId)
                .ToMessage<OrderBilled>(message => message.OrderId);
        }

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"OrderPlaced message received.");
            Data.IsOrderPlaced = true;
            return ProcessOrder(context);
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"OrderBilled message received.");
            Data.IsOrderBilled = true;
            return ProcessOrder(context);
        }

        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsOrderPlaced && Data.IsOrderBilled)
            {
                await context.SendLocal(new ShipOrder() { OrderId = Data.OrderId });
                MarkAsComplete();
            }
        }
    }
}
