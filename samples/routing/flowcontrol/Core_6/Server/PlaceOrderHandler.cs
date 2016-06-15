using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Thread.Sleep(1000);
        log.Info($"Order {message.OrderId} accepted.");
        return Task.FromResult(0);
    }

    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
}