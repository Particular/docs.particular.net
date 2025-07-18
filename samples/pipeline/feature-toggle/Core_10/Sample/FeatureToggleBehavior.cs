using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using Microsoft.Extensions.DependencyInjection;

#region FeatureToggleBehavior
class FeatureToggleBehavior(IList<Func<IInvokeHandlerContext, bool>> toggles) : Behavior<IInvokeHandlerContext>
{
    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        if (toggles.All(toggle => toggle(context)))
        {
            await next();
        }
        else
        {
            var logger = context.Builder.GetService<ILogger<FeatureToggleBehavior>>();
            logger.LogInformation("Feature toggle skipped execution of handler: {HandlerType}", context.MessageHandler.HandlerType);
        }
    }
}

#endregion