using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region CacheInvalidationHandler
namespace DataDistribution
{
    class CacheInvalidationHandler :
        IHandleMessages<OrderAccepted>
    {
        static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
        public Task Handle(OrderAccepted message, IMessageHandlerContext context)
        {
            log.Info("Invalidating cache.");
            return Task.FromResult(0);
        }
    }
}
#endregion