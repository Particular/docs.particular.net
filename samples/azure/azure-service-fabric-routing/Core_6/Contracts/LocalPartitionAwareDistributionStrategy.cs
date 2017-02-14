using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Routing;

namespace Contracts
{
    public class LocalPartitionAwareDistributionStrategy : DistributionStrategy
    {
        readonly string _partitionId;

        public LocalPartitionAwareDistributionStrategy(string partitionId, string endpoint, DistributionStrategyScope scope)
            : base(endpoint, scope)
        {
            _partitionId = partitionId;
        }

        public override string SelectReceiver(string[] receiverAddresses)
        {
            throw new NotImplementedException();
        }

        public override string SelectDestination(DistributionContext context)
        {
            context.Headers[PartitionHeaders.PartitionKey] = _partitionId;

            //var logicalAddress = LogicalAddress.CreateRemoteAddress(new EndpointInstance(Endpoint, _partitionId));
            return context.ReceiverAddresses.First();//.Single(a => a == logicalAddress.ToString());
        }
    }
}