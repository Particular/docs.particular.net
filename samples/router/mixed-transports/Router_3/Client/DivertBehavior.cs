using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class DivertBehavior : Behavior<IRoutingContext>
{
    public async override Task Invoke(IRoutingContext context, Func<Task> next)
    {
        if (!context.Extensions.TryGet<string>("Tenant", out var tenant))
        {
            await next();
            return;
        }

        if (tenant != "test")
        {
            await next();
            return;
        }

        var newStrategies = context.RoutingStrategies.Select(s => new RedirectRoutingStrategy(s)).ToList();
        context.RoutingStrategies = newStrategies;

        await next();
    }
}