using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Store.Messages.Commands;

public class OrdersHub :
    Hub
{
    static int orderNumber;
    private IEndpointInstance endpoint;

    public OrdersHub(IEndpointInstance endpoint)
    {
        this.endpoint = endpoint;
    }
    public void CancelOrder(int orderNumber, bool isDebug)
    {
        var command = new CancelOrder
        {
            ClientId = Context.ConnectionId,
            OrderNumber = orderNumber
        };
        
        var sendOptions = new SendOptions();
        sendOptions.SetHeader("Debug", isDebug.ToString());
        endpoint.Send(command, sendOptions);
    }

    public void PlaceOrder(string[] productIds, bool isDebug)
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
        endpoint.Send(command, sendOptions);
    }
}