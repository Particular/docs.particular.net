using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} accepted.");
        return context.Publish(new OrderAccepted
        {
            OrderId = message.OrderId,
            Value = message.Value
        });
    }

    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
}