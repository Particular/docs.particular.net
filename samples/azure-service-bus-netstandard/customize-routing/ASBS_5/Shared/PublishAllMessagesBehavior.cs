using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Routing;

public class PublishAllMessagesBehavior : Behavior<IRoutingContext>
{
    public override Task Invoke(IRoutingContext context, Func<Task> next)
    {
        if (context.Extensions.TryGet(out State _))
        {
            //Override the routing strategies if the message comes from the user code
            var logicalMessage = context.Extensions.Get<OutgoingLogicalMessage>();
            var newRoutingStrategies =
                context.RoutingStrategies.Select(x => new MulticastRoutingStrategy(logicalMessage.MessageType));
            context.RoutingStrategies = newRoutingStrategies.ToList();
        }

        return next();
    }

    public class State
    {
    }
}