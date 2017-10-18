using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Shipping
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            // Process these ones fast
            return Task.CompletedTask;
        }
    }
}