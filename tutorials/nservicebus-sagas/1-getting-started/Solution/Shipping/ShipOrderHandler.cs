using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Shipping
{
    class ShipOrderHandler : IHandleMessages<ShipOrder>
    {
        static ILog log = LogManager.GetLogger<ShipOrderHandler>();

        public Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order [{message.OrderId}] - Succesfully shipped.");
            return Task.CompletedTask;
        }
    }
}
