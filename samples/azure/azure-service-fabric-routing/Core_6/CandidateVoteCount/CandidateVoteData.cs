using NServiceBus;

public class CandidateVoteData : ContainSagaData
{
    public bool Started { get; set; }
    public string Candidate { get; set; }
    public int Count { get; set; }
}