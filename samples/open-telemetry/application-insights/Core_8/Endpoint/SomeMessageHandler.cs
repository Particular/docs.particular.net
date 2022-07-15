using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class SomeMessageHandler : IHandleMessages<SomeMessage>
{
    static ILog log = LogManager.GetLogger<SomeMessageHandler>();
    static Random random = new Random();

    public async Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        await Task.Delay(random.Next(50, 250), context.CancellationToken);

        if (random.Next(10) <= 1)
        {
            throw new Exception("Random 10% chaos!");
        }

        log.Info("Message handled");
    }
}