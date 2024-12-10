using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region message-handler

class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
    
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        var tenant = context.MessageHeaders["tenant_id"];

        log.Info($"Processing PlaceOrder message for tenant {tenant}");

        return context.Publish(new OrderAccepted());
    }
}

#endregion