using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    static List<Guid> wasMessageDelayed = new List<Guid>();

    #region PlaceOrderHandler
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        if (ShouldMessageBeDelayed(message.Id))
        {
            SendOptions options = new SendOptions();

            options.DelayDeliveryWith(TimeSpan.FromSeconds(5));
            options.RouteToThisEndpoint();
            await context.Send(message, options);
            Console.WriteLine(@"[Defer Message Handling] Deffering Message with Id: {0}", message.Id);
            return;
        }

        Console.WriteLine(@"[Defer Message Handling] Order for Product:{0} placed with id: {1}", message.Product, message.Id);
    }
    #endregion

    private bool ShouldMessageBeDelayed(Guid id)
    {
        if (wasMessageDelayed.Contains(id))
        {
            return false;
        }

        wasMessageDelayed.Add(id);
        return true;
    }
}
