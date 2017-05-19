using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

class MyService
{
    static ILog log = LogManager.GetLogger<MyService>();

    public void DoOneThing(decimal value)
    {
        var owner = Thread.CurrentPrincipal.Identity.Name;
        log.Info($"Got order worth {value:C} owned by {owner}.");
    }
}