using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region dispatch-notification-behavior
class DispatchNotificationBehavior(List<IDispatchNotifier> watches) :
    Behavior<IDispatchContext>
{
    public override async Task Invoke(IDispatchContext context, Func<Task> next)
    {
        await next();

        await Task.WhenAll(watches.Select(watch => watch.Notify(context.Operations)));
    }
}
#endregion