using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Routing;

namespace Contracts
{
    public class LocalPartitionAwareDistributionStrategy : DistributionStrategy
    {
        readonly string _partitionId;
        LogicalAddress _logicalAddress;

        public LocalPartitionAwareDistributionStrategy(string partitionId, string endpoint, DistributionStrategyScope scope)
            : base(endpoint, scope)
        {
            _partitionId = partitionId;
            _logicalAddress = LogicalAddress.CreateRemoteAddress(new EndpointInstance(endpoint, _partitionId));
        }

        public override string SelectReceiver(string[] receiverAddresses)
        {
            throw new NotImplementedException();
        }

        public override string SelectDestination(DistributionContext context)
        {
            context.Headers[PartitionHeaders.PartitionKey] = _partitionId;
            return context.ReceiverAddresses.Single(a => a == _logicalAddress.ToString());
        }
    }
}