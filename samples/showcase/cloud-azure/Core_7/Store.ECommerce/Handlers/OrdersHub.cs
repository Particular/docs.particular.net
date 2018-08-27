using System.Threading;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using System.Diagnostics;
using System.Threading.Tasks;
using Store.Messages.Commands;

public class OrdersHub :
    Hub
{
    static int orderNumber;

    public async Task CancelOrder(int orderNumber)
    {
        var command = new CancelOrder
        {
            ClientId = Context.ConnectionId,
            OrderNumber = orderNumber
        };

        var isDebug = (bool)Clients.Caller.debug;
        var sendOptions = new SendOptions();
        sendOptions.SetHeader("Debug", isDebug.ToString());
        await MvcApplication.EndpointInstance.Send(command,sendOptions);
    }

    public async Task PlaceOrder(string[] productIds)
    {
        var isDebug = (bool)Clients.Caller.debug;
        if (isDebug)
        {
            Debugger.Break();
        }

        var command = new SubmitOrder
        {
            ClientId = Context.ConnectionId,
            OrderNumber = Interlocked.Increment(ref orderNumber),
            ProductIds = productIds,
            // This property will be encrypted. Therefore when viewing the message in the queue, the actual values will not be shown.
            CreditCardNumber = "4000 0000 0000 0008",
            // This property will be encrypted.
            ExpirationDate = "10/13"
        };

        var sendOptions = new SendOptions();
        sendOptions.SetHeader("Debug", isDebug.ToString());
        await MvcApplication.EndpointInstance.Send(command, sendOptions);
    }
}