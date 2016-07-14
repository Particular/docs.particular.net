using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region OrderCreatedHandler
public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
{
    static ILog log = LogManager.GetLogger<OrderCreatedHandler>();

    public void Handle(OrderPlaced message)
    {
    }

    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        log.Info($"Handling: OrderPlaced for Order Id: {message.OrderId}");
        return Task.FromResult(0);
    }
}
#endregion