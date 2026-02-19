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

        logger.LogInformation("ShipWithAlpineHandler: Delaying Order [{orderId}] {WaitingTime} seconds.", message.OrderId, waitingTime);

        await Task.Delay(waitingTime * 1000, CancellationToken.None);

        await context.Reply(new ShipmentAcceptedByAlpine());
    }
}

#endregion