using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Sales
{
    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrder>
    {
        SimulationEffects simulationEffects;

        public PlaceOrderHandler(SimulationEffects simulationEffects)
        {
            this.simulationEffects = simulationEffects;
        }

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            // Simulate the time taken to process a message
            await simulationEffects.SimulateMessageProcessing()
                .ConfigureAwait(false);

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            await context.Publish(orderPlaced)
                .ConfigureAwait(false);
        }
    }
}