using System;
using NServiceBus;
#region OrderCreatedHandler
public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
{
    public void Handle(OrderPlaced message)
    {
        Console.WriteLine(@"Handling: OrderPlaceed for Order Id: {0}", message.OrderId);
    }
}
#endregion