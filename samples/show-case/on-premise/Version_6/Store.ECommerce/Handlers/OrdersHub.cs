﻿using System.Threading;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using System.Diagnostics;
using Store.Messages.Commands;

public class OrdersHub : Hub
{
    static int orderNumber;

    public void CancelOrder(int orderNumber)
    {
        CancelOrder command = new CancelOrder
        {
            ClientId = Context.ConnectionId,
            OrderNumber = orderNumber
        };

        bool isDebug = (bool)Clients.Caller.debug;
        SendOptions sendOptions = new SendOptions();
        sendOptions.SetHeader("Debug", isDebug.ToString());
        MvcApplication.Endpoint.Send(command,sendOptions);
    }

    public void PlaceOrder(string[] productIds)
    {
        bool isDebug = (bool)Clients.Caller.debug;
        if (isDebug)
        {
            Debugger.Break();
        }

        SubmitOrder command = new SubmitOrder
        {
            ClientId = Context.ConnectionId,
            OrderNumber = Interlocked.Increment(ref orderNumber),
            ProductIds = productIds,
            // This property will be encrypted. Therefore when viewing the message in the queue, the actual values will not be shown. 
            EncryptedCreditCardNumber = "4000 0000 0000 0008",
            // This property will be encrypted.
            EncryptedExpirationDate = "10/13" 
        };

        SendOptions sendOptions = new SendOptions();
        sendOptions.SetHeader("Debug", isDebug.ToString());
        MvcApplication.Endpoint.Send(command, sendOptions);
    }
}