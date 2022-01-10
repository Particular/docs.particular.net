using NServiceBus;
using NServiceBus.Logging;
using Messages;
using System;
using System.Threading.Tasks;

namespace Shipping.Integration
{
    #region ShipWithAlpineHandler

    class ShipWithAlpineHandler : IHandleMessages<ShipWithAlpine>
    {
        static ILog log = LogManager.GetLogger<ShipWithAlpineHandler>();

        const int MaximumTimeAlpineMightRespond = 30;
        static Random random = new Random();

        public async Task Handle(ShipWithAlpine message, IMessageHandlerContext context)
        {
            var waitingTime = random.Next(MaximumTimeAlpineMightRespond);

            log.Info($"ShipWithAlpineHandler: Delaying Order [{message.OrderId}] {waitingTime} seconds.");

            await Task.Delay(waitingTime * 1000);

            await context.Reply(new ShipmentAcceptedByAlpine());
        }
    }

    #endregion
}