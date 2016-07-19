using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
    static List<Guid> wasMessageDelayed = new List<Guid>();

    #region PlaceOrderHandler
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        if (ShouldMessageBeDelayed(message.Id))
        {
            var options = new SendOptions();

            options.DelayDeliveryWith(TimeSpan.FromSeconds(5));
            options.RouteToThisEndpoint();
            await context.Send(message, options)
                .ConfigureAwait(false);
            log.Info($"[Defer Message Handling] Deferring Message with Id: {message.Id}");
            return;
        }

        log.Info($"[Defer Message Handling] Order for Product:{message.Product} placed with id: {message.Id}");
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
