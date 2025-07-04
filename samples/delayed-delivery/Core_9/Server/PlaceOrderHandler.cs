using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class PlaceOrderHandler(ILogger<PlaceDelayedOrderHandler> logger) :
    IHandleMessages<PlaceOrder>
{

    static List<Guid> wasMessageDelayed = new List<Guid>();

    #region PlaceOrderHandler
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        if (ShouldMessageBeDelayed(message.Id))
        {
            var options = new SendOptions();

            options.DelayDeliveryWith(TimeSpan.FromSeconds(5));
            options.RouteToThisEndpoint();
            await context.Send(message, options);
            logger.LogInformation("[Defer Message Handling] Deferring Message with Id: {Id}", message.Id);
            return;
        }

        logger.LogInformation("[Defer Message Handling] Order for Product:{Product} placed with id: {Id}", message.Product, message.Id);
    }
    #endregion

    bool ShouldMessageBeDelayed(Guid id)
    {
        if (wasMessageDelayed.Contains(id))
        {
            return false;
        }

        wasMessageDelayed.Add(id);
        return true;
    }
}
