using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace DataDistribution
{
    class CacheInvalidationHandler :
        IHandleMessages<OrderAccepted>
    {
        static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
        public Task Handle(OrderAccepted message, IMessageHandlerContext context)
        {
            log.Info("Invalidating cache.");
            return Task.CompletedTask;
        }
    }
}
