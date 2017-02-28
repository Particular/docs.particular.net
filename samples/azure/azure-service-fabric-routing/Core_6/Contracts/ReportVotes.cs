using NServiceBus;

public class ReportVotes :
    ICommand
{
    public string Candidate { get; set; }
    public int NumberOfVotes { get; set; }
}