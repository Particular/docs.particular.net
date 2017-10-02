using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region dispatch-notification-behavior
class DispatchNotificationBehavior :
    Behavior<IDispatchContext>
{
    List<IWatchDispatches> watches;

    public DispatchNotificationBehavior(List<IWatchDispatches> watches)
    {
        this.watches = watches;
    }

    public override async Task Invoke(IDispatchContext context, Func<Task> next)
    {
        await next()
            .ConfigureAwait(false);
        var tasks = watches.Select(watch => watch.Notify(context.Operations));
        await Task.WhenAll(tasks)
            .ConfigureAwait(false);
    }
}
#endregion