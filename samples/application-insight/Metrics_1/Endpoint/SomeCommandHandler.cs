using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class SomeCommandHandler :
    IHandleMessages<SomeCommand>
{
    static ILog log = LogManager.GetLogger<SomeCommandHandler>();
    static readonly Random r = new Random();

    public async Task Handle(SomeCommand message, IMessageHandlerContext context)
    {
        await Task.Delay(r.Next(200, 2000))
            .ConfigureAwait(false);

        if (r.Next(10) <= 1) throw new Exception("Random 10% chaos!");

        log.Info("Hello from SomeCommandHandler");
    }
}