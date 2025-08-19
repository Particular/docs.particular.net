namespace Core.Pipeline;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Pipeline;

#region InjectingDependencies

public class BehaviorUsingDependencyInjection :
    Behavior<IIncomingLogicalMessageContext>
{
    // Dependencies injected into the constructor are singletons and cached for the lifetime
    // of the endpoint
    public BehaviorUsingDependencyInjection(SingletonDependency singletonDependency)
    {
        this.singletonDependency = singletonDependency;
    }

    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        var scopedDependency = context.Builder.GetRequiredService<ScopedDependency>();
        // do something with the scoped dependency before
        await next();
        // do something with the scoped dependency after
    }

    SingletonDependency singletonDependency;
}

#endregion

public class SingletonDependency
{
}

public class ScopedDependency
{
}