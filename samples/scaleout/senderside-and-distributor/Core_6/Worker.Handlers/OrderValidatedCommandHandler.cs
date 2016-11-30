using System.Reflection;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderValidatedCommandHandler :
    IHandleMessages<OrderValidated>
{
    static ILog log = LogManager.GetLogger<OrderValidatedCommandHandler>();

    public async Task Handle(OrderValidated message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} validated. Sending notification to the customer.");

        await Task.Delay(1000).ConfigureAwait(false);

        var placed = new PlaceOrderResponse
        {
            OrderId = message.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };

        var options = new SendOptions();
        options.SetDestination(message.Sender);
        await context.Send(placed, options)
            .ConfigureAwait(false);
    }
}