using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus.Routing;

class ForwardingAddressDirectory
{
    List<Tuple<Type, UnicastRoute>> forwardingRoutes = new List<Tuple<Type, UnicastRoute>>();

    public void ForwardToRoute(Type messageType, UnicastRoute route)
    {
        forwardingRoutes.Add(Tuple.Create(messageType, route));
    }

    public ILookup<Type, UnicastRoute> ToLookup()
        => forwardingRoutes.ToLookup(
            x => x.Item1,
            x => x.Item2
        );
}