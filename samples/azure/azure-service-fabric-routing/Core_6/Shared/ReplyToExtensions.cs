using NServiceBus;
using NServiceBus.Routing;

namespace Shared
{
    static class ReplyToExtensions
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