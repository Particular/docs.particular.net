using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    private static readonly ILogger<PlaceOrderHandler> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<PlaceOrderHandler>();

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
            logger.LogInformation($"[Defer Message Handling] Deferring Message with Id: {message.Id}");
            return;
        }

        logger.LogInformation($"[Defer Message Handling] Order for Product:{message.Product} placed with id: {message.Id}");
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
