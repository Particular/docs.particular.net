namespace Core_7.Reply
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;

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
            #region Replay
            await context.Reply(new ShipmentAcceptedByMaple())
                    .ConfigureAwait(false);
            #endregion
        }
    }

    internal class ShipmentAcceptedByMaple
    {
        public ShipmentAcceptedByMaple()
        {
        }
    }

    internal class ShipWithMaple
    {
        public object OrderId { get; internal set; }
    }
}