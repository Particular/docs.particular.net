using System;
using System.Threading.Tasks;
using NServiceBus;
#region OrderCreatedHandler
public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message)
    {
        Console.WriteLine(@"Handling: OrderPlaceed for Order Id: {0}", message.OrderId);
        return Task.FromResult(0);
    }
}
#endregion