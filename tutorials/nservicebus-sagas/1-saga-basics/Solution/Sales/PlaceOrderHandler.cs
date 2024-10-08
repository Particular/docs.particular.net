﻿using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) :
    IHandleMessages<PlaceOrder>
{
    static Random random = new Random();

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {message.OrderId}", message.OrderId);

        // This is normally where some business logic would occur

        // Uncomment to test throwing a systemic exception
        //throw new Exception("BOOM");

        // Uncomment to test throwing a transient exception
        //if (random.Next(0, 5) == 0)
        //{
        //    throw new Exception("Oops");
        //}

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };
        return context.Publish(orderPlaced);
    }
}