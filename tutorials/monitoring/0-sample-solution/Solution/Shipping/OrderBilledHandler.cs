using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Shipping
{
    public class OrderBilledHandler :
        IHandleMessages<OrderBilled>
    {
        SimulationEffects simulationEffects;

        public OrderBilledHandler(SimulationEffects simulationEffects)
        {
            this.simulationEffects = simulationEffects;
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            await simulationEffects.SimulateMessageProcessing()
                .ConfigureAwait(false);
        }
    }
}