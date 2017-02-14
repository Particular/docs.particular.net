namespace CandidateVoteCount
{
    using System;
    using Contracts;

    class ZipCodePartitionDistributionBehavior : PartitionAwareOutgoingBehavior
    {
        protected override string MapMessageToPartition(object message)
        {
            var trackZipCode = message as TrackZipCode;
            if (trackZipCode != null)
            {
                var zipCodeAsNumber = Convert.ToInt32(trackZipCode.ZipCode);
                // 00000..33000 => 33000 34000..66000 => 66000 67000..99000 => 99000
                if (zipCodeAsNumber >= 0 && zipCodeAsNumber <= 33000)
                {
                    return "33000";
                }

                if (zipCodeAsNumber >= 34000 && zipCodeAsNumber <= 66000)
                {
                    return "66000";
                }

                if (zipCodeAsNumber >= 67000 && zipCodeAsNumber <= 99000)
                {
                    return "99000";
                }

                throw new Exception($"Invalid zip code '{zipCodeAsNumber}' for message of type '{message.GetType()}'.");
            }

            throw new Exception($"No partition mapping is found for message type '{message.GetType()}'.");
        }
    }
}