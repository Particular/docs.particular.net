using System;
using System.Threading.Tasks;
using NServiceBus;

class MyMessageHandler : IHandleMessages<ProcessOrder>, IHandleMessages<ShipOrder>
{
    #region handlers
    public Task Handle(ProcessOrder message, IMessageHandlerContext context)
    {
        context.MessageHeaders.TryGetValue("CustomerId", out var customerId);
        Console.WriteLine($"Recieved ProcessOrder for customer {customerId}. Shipping");

        return context.SendLocal(new ShipOrder());
    }

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        context.MessageHeaders.TryGetValue("CustomerId", out var customerId);
        Console.WriteLine($"Shipping Order for customer {customerId}.");

        return Task.CompletedTask;
    }
    #endregion
}
