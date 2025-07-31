using System.Diagnostics;

#region add-tags-from-handler
class ShipOrderHandler : IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Order shipped {message.OrderId}");
        // Figure out what state we are shipping to
        Activity.Current?.AddTag("sample.shipping.state", "STATE");
        return Task.CompletedTask;
    }
}
#endregion
