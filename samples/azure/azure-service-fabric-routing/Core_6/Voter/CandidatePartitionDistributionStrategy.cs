namespace Voter
{
    using System;
    using Contracts;
    using NServiceBus;

    class CandidatePartitionDistributionStrategy : PartitionAwareDistributionStrategy
    {
        public CandidatePartitionDistributionStrategy(DistributionStrategyScope scope) : base("CandidateVoteCount", scope)
        {
        }

        protected override string MapMessageToPartition(object message)
        {
            var candidate = message as PlaceVote;
            if (candidate != null)
            {
                return candidate.Candidate;
            }

            throw new Exception($"No partition mapping is found for message type '{message.GetType()}'.");
        }
    }
}