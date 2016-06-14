using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Routing;
using NServiceBus.Transports;
using NServiceBus.Unicast.Messages;

class AutomaticPublishRouter : UnicastRouter
{

    public AutomaticPublishRouter(UnicastRoutingTable unicastRoutingTable, MessageMetadataRegistry messageMetadataRegistry, EndpointInstances endpointInstances, TransportAddresses physicalAddresses) 
        : base(messageMetadataRegistry, endpointInstances, physicalAddresses)
    {
        this.unicastRoutingTable = unicastRoutingTable;
    }

    protected override Task<IEnumerable<IUnicastRoute>> GetDestinations(ContextBag contextBag, List<Type> typesToRoute)
    {
        return unicastRoutingTable.GetDestinationsFor(typesToRoute, contextBag);
    }

    UnicastRoutingTable unicastRoutingTable;
}