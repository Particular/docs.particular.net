using NServiceBus;
using NServiceBus.Logging;
using Messages;
using System;
using System.Threading.Tasks;

namespace Shipping.Integration
{
    class ShipWithMapleHandler : 
        IHandleMessages<ShipWithMaple>
    {
        static ILog log = LogManager.GetLogger<ShipWithMapleHandler>();

        const int MaximumTimeMapleMightRespond = 60;
        static Random rnd = new Random();

        public async Task Handle(ShipWithMaple message, IMessageHandlerContext context)
        {
            var waitingTime = rnd.Next(MaximumTimeMapleMightRespond);

            log.Info($"Order #{message.OrderId} - Waiting {waitingTime} seconds.");
            await Task.Delay(waitingTime * 1000)
                .ConfigureAwait(false);

            await context.Reply(new ShipmentAcceptedByMaple())
                .ConfigureAwait(false);
        }
    }
}