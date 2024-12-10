using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;


class OrderAcceptedHandler :
    IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
    
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        var tenant = context.MessageHeaders["tenant_id"];

        log.Info($"Processing OrderAccepted message for tenant {tenant}");

        return Task.CompletedTask;
    }
}
