using Contracts;
using Microsoft.Extensions.Logging;

#region PlaceOrderHandler
sealed class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger)
    :IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine("In the handler");
        logger.LogInformation("""
            Published PlaceOrder 
                OrderId  = {OrderId},
                Product  = {Product},
                Quantity = {Quantity}           
            """,
            message.OrderId,
            message.Product,
            message.Quantity);
    }
}
#endregion
