using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Store.Messages.Commands;

public class OrdersHub : Hub
{
    private static int orderNumber;
    private IMessageSession messageSession;

    public OrdersHub(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public async Task CancelOrder(int orderNumber, bool isDebug)
    {
        var command = new CancelOrder
        {
            ClientId = Context.ConnectionId,
            OrderNumber = orderNumber
        };
        
        var sendOptions = new SendOptions();
        sendOptions.SetHeader("Debug", isDebug.ToString());
        await messageSession.Send(command, sendOptions);
    }

    public async Task PlaceOrder(string[] productIds, bool isDebug)
    {
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
        await messageSession.Send(command, sendOptions);
    }
}