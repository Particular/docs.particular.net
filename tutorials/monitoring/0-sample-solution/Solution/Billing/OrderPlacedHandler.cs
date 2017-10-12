using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Billing
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
        SimulationEffects simulationEffects;

        public OrderPlacedHandler(SimulationEffects simulationEffects)
        {
            this.simulationEffects = simulationEffects;
        }

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            await simulationEffects.SimulatedMessageProcessing()
                .ConfigureAwait(false);

            var orderBilled = new OrderBilled
            {
                OrderId = message.OrderId
            };
            await context.Publish(orderBilled)
                .ConfigureAwait(false);
        }
    }
}