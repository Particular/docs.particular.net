namespace Contracts
{
    using System;

    public static class PartitionMap
    {
        // This could be done in a clever way
        public static string Map(object message)
        {
            var candidate = message as IHaveCandidate;
            if (candidate != null)
            {
                return CandidateMappings.Map(candidate);
            }

            var zipCode = message as IHaveZipCode;
            if (zipCode != null)
            {
                return ZipCodeMappings.Map(zipCode);
            }

            throw new InvalidOperationException();
        }
    }
}