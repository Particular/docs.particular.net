using NServiceBus;
using NServiceBus.Logging;
using Messages;
using System;
using System.Threading.Tasks;

namespace Shipping.Integration
{
    class ShipWithAlpineHandler : IHandleMessages<ShipWithAlpine>
    {
        static ILog log = LogManager.GetLogger<ShipWithAlpineHandler>();

        const int MaximumTimeMapleMightRespond = 30;
        static Random random = new Random();

        public async Task Handle(ShipWithAlpine message, IMessageHandlerContext context)
        {
            var waitingTime = random.Next(MaximumTimeMapleMightRespond);

            log.Info($"Order #{message.OrderId} - Waiting {waitingTime} seconds.");

            await Task.Delay(waitingTime * 1000)
                .ConfigureAwait(false);

            await context.Reply(new ShipmentAcceptedByAlpine())
                .ConfigureAwait(false);
        }
    }
}