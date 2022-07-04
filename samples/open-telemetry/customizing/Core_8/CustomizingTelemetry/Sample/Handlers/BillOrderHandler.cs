class BillOrderHandler : IHandleMessages<BillOrder>
{
    public Task Handle(BillOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Order billed {message.OrderId}");
        return Task.CompletedTask;
    }
}