using NServiceBus.Routing;

namespace Contracts
{
    using System;
    using NServiceBus;

    static class SendOptionsExtensions
    {
        public static void RouteReplyToPartitionedEndpoint(this SendOptions options, string endpointname, string partitionKey)
        {
            var logicalAddress = LogicalAddress.CreateRemoteAddress(new EndpointInstance(endpointname));

            var endpointInstanceAddress = logicalAddress.CreateIndividualizedAddress(partitionKey);

            options.SetHeader(PartitionHeaders.PartitionKey, partitionKey);

            options.RouteReplyTo(endpointInstanceAddress.ToString());
        }
    }
}