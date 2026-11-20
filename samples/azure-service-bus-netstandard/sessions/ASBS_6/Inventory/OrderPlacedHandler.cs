using Shared;

namespace Inventory;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) : IHandleMessages<OrderPlaced>
{
    public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        //Inventory is run by a legacy mainframe code that expects multiple messages
        //to be sent strictly in order
        var inventoryTransactionId = Guid.NewGuid().ToString();

        var beginSendOptions = new SendOptions();
        beginSendOptions.SetSessionId(inventoryTransactionId);
        beginSendOptions.RequireImmediateDispatch();
        await context.Send(new BeginTransaction(), beginSendOptions);

        for (var i = 0; i < message.NumberOfItems; i++)
        {
            var reserveSendOptions = new SendOptions();
            reserveSendOptions.SetSessionId(inventoryTransactionId);
            reserveSendOptions.RequireImmediateDispatch();
            await context.Send(new Reserve
            {
                OrderId = message.OrderId
            }, reserveSendOptions);
        }

        var completeSendOptions = new SendOptions();
        completeSendOptions.SetSessionId(inventoryTransactionId);
        completeSendOptions.RequireImmediateDispatch();
        await context.Send(new CompleteTransaction(), completeSendOptions);

        logger.LogInformation("Processing OrderPlaced, inventory reserved for {order}", message.OrderId);
    }
}