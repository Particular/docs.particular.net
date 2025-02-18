using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.Lesson1.Intro;
#pragma warning disable 1998

#region EmptyShippingPolicy
public class ShippingPolicy(ILogger<ShippingPolicy> logger) :
    IHandleMessages<OrderPlaced>,
    IHandleMessages<OrderBilled>
{
   public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderPlaced, OrderId = {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderBilled, OrderId = {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}
#endregion

#pragma warning restore 1998