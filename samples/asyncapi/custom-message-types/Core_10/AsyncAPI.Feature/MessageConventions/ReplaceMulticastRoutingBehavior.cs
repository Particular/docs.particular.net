using NServiceBus.Pipeline;
using NServiceBus.Routing;

namespace AsyncAPI.Feature;

class ReplaceMulticastRoutingBehavior(Dictionary<Type, Type> publishedEventCache) : IBehavior<IRoutingContext, IRoutingContext>
{
    public Task Invoke(IRoutingContext context, Func<IRoutingContext, Task> next)
    {
        var logicalMessage = context.Extensions.Get<OutgoingLogicalMessage>();
        if (publishedEventCache.TryGetValue(logicalMessage.MessageType, out var publishedEvent))
        {
            var newStrategies = new List<RoutingStrategy>(context.RoutingStrategies.Count);
            var strategies = context.RoutingStrategies;
            foreach (var strategy in strategies)
            {
                if (strategy is MulticastRoutingStrategy multicastRoutingStrategy)
                {
                    // we assume here a multi cast address tag will never do anything with the headers so we pass a static empty dictionary
                    var multicastAddressTag = (MulticastAddressTag) multicastRoutingStrategy.Apply(emptyHeaders);
                    if (multicastAddressTag.MessageType == logicalMessage.MessageType)
                    {
                        newStrategies.Add(new MulticastRoutingStrategy(publishedEvent));
                    }
                }
                else
                {
                    newStrategies.Add(strategy);
                }
            }

            context.RoutingStrategies = newStrategies;
        }

        return next(context);
    }

    private readonly Dictionary<string, string> emptyHeaders = new Dictionary<string, string>();
}