using NServiceBus;

namespace Voter
{
    using System;
    using Contracts;

    class CandidatePartitionDistributionStrategy : PartitionAwareDistributionStrategy
    {
        protected override string MapMessageToPartition(object message)
        {
            var candidate = message as PlaceVote;
            if (candidate != null)
            {
                return candidate.Candidate;
            }

            throw new Exception($"No partition mapping is found for message type '{message.GetType()}'.");
        }

        public CandidatePartitionDistributionStrategy(string endpoint, DistributionStrategyScope scope) : base(endpoint, scope)
        {
        }
    }
}