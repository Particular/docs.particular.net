﻿using NServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;

namespace Shipping.Integration;

#region ShipWithAlpineHandler

class ShipWithAlpineHandler(ILogger<ShipWithAlpineHandler> logger) : IHandleMessages<ShipWithAlpine>
{
    const int MaximumTimeAlpineMightRespond = 30;

    public async Task Handle(ShipWithAlpine message, IMessageHandlerContext context)
    {
        var waitingTime = Random.Shared.Next(MaximumTimeAlpineMightRespond);

        logger.LogInformation($"ShipWithAlpineHandler: Delaying Order [{message.OrderId}] {waitingTime} seconds.");

        await Task.Delay(waitingTime * 1000, CancellationToken.None);

        await context.Reply(new ShipmentAcceptedByAlpine());
    }
}

#endregion