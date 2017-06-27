using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class SetCultureBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        string uiCulture;

        if (context.Headers.TryGetValue("Culture", out uiCulture))
        {
            var oldCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(uiCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(uiCulture);
            await next().ConfigureAwait(false);
            Thread.CurrentThread.CurrentUICulture = oldCulture;
            Thread.CurrentThread.CurrentCulture = oldCulture;
        }
        else
        {
            await next();
        }
    }
}