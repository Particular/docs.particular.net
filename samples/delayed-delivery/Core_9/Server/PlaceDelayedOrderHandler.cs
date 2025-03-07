﻿using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
#region PlaceDelayedOrderHandler

public class PlaceDelayedOrderHandler(ILogger<PlaceDelayedOrderHandler> logger) :
    IHandleMessages<PlaceDelayedOrder>
{   

    public Task Handle(PlaceDelayedOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation($"[Defer Message Delivery] Order for Product:{message.Product} placed with id: {message.Id}");
        return Task.CompletedTask;
    }
}

#endregion
