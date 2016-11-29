using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class ProcessOrderCommandHandler :
    IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<ProcessOrderCommandHandler>();

    #region SendLocal

    public async Task Handle(PlaceOrder placeOrder, IMessageHandlerContext context)
    {
        log.Info($"Sending order {placeOrder.OrderId} to validation.");

        await Task.Delay(1000).ConfigureAwait(false);

        var validateOrder = new ValidateOrder
        {
            OrderId = placeOrder.OrderId,
            Sender = context.ReplyToAddress
        };


        await context.SendLocal(validateOrder)
            .ConfigureAwait(false);
    }

    #endregion
}
