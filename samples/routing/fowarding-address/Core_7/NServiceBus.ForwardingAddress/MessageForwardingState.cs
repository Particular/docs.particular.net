using System.Collections.Generic;
using NServiceBus.Routing;

class MessageForwardingState
{
    List<UnicastRoutingStrategy> routingStrategies = new List<UnicastRoutingStrategy>();

    public void AddRoutingStrategy(UnicastRoutingStrategy forwardingRoutingStrategy)
    {
        routingStrategies.Add(forwardingRoutingStrategy);
    }

    public IReadOnlyCollection<RoutingStrategy> GetRoutingStrategies()
    {
        return routingStrategies;
    }
}