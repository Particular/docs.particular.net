using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region FeatureToggleBehavior
class FeatureToggleBehavior :
    Behavior<IInvokeHandlerContext>
{
    static ILog log = LogManager.GetLogger<FeatureToggleBehavior>();

    IList<Func<IInvokeHandlerContext, bool>> toggles;

    public FeatureToggleBehavior(IList<Func<IInvokeHandlerContext, bool>> toggles)
    {
        this.toggles = toggles;
    }

    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        if (toggles.All(toggle => toggle(context)))
        {
            await next().ConfigureAwait(false);
        }
        else
        {
            log.InfoFormat("Feature toggle skipped execution of handler: {0}", context.MessageHandler.HandlerType, context.MessageHandler);
        }
    }
}

#endregion