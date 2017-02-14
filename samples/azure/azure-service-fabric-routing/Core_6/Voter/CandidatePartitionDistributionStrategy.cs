namespace Voter
{
    using System;
    using Contracts;
    using NServiceBus;

    public class CandidatePartitionDistributionStrategy : PartitionAwareDistributionStrategy
    {
        public CandidatePartitionDistributionStrategy(DistributionStrategyScope scope) : base("CandidateVoteCount", scope)
        {
        }

        public override string MapMessageToPartition(object message)
        {
            var candidate = message as PlaceVote;
            if (candidate != null)
            {
                return candidate.Candidate;
            }

            throw new InvalidOperationException($"No partition mapping is found for message type '{message.GetType()}'.");
        }
    }
}