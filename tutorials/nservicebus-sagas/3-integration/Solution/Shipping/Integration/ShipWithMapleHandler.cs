using NServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Messages;

namespace Shipping.Integration;

#region ShipWithMapleHandler

class ShipWithMapleHandler(ILogger<ShipWithMapleHandler> logger) : IHandleMessages<ShipWithMaple>
{
    const int MaximumTimeMapleMightRespond = 60;

    public async Task Handle(ShipWithMaple message, IMessageHandlerContext context)
    {
        var waitingTime = Random.Shared.Next(MaximumTimeMapleMightRespond);

        logger.LogInformation("ShipWithMapleHandler: Delaying Order [{OrderId}] {WaitingTime} seconds.", message.OrderId, waitingTime);

        await Task.Delay(waitingTime * 1000, CancellationToken.None);

        await context.Reply(new ShipmentAcceptedByMaple());
    }
}

#endregion