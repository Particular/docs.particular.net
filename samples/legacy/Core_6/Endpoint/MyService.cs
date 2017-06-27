using System.Threading;
using NServiceBus.Logging;

class MyService
{
    static ILog log = LogManager.GetLogger<MyService>();

    IBus bus;

    public MyService(IBus bus)
    {
        this.bus = bus;
    }

    public void DoOneThing(decimal value)
    {
        var owner = Thread.CurrentPrincipal.Identity.Name;
        log.Info($"Got order with ID {bus.MessageId} worth {value:C} owned by {owner}.");
    }
}