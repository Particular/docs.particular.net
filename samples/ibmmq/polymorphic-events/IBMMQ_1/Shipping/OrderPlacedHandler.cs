#region OrderPlacedHandler
sealed class OrderPlacedHandler : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        var messageType = message.GetType().Name;
        Console.WriteLine($"Received {messageType}: OrderId={message.OrderId}, Product={message.Product}");
        return Task.CompletedTask;
    }
}
#endregion
