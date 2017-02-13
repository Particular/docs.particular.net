namespace Messages
{
    public static class CandidateMappings
    {
        public static string Map(IHaveCandidate candidate)
        {
            return candidate.Candidate;
        }
    }
}