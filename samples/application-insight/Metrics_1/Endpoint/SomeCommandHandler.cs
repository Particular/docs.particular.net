using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class SomeCommandHandler :
    IHandleMessages<SomeCommand>
{
    static ILog log = LogManager.GetLogger<SomeCommandHandler>();
    static Random random = new Random();

    public async Task Handle(SomeCommand message, IMessageHandlerContext context)
    {
        await Task.Delay(random.Next(50, 250))
            .ConfigureAwait(false);

        if (random.Next(10) <= 1)
        {
            throw new Exception("Random 10% chaos!");
        }

        log.Info("Hello from SomeCommandHandler");
    }
}