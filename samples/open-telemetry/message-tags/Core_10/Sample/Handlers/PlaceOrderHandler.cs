using NServiceBus;

class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Order placed. OrderId={message.OrderId}, CustomerId={message.CustomerId}");
        return Task.CompletedTask;
    }
}
