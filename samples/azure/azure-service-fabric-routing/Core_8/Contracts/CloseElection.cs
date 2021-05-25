using NServiceBus;

public class CloseElection :
    ICommand
{
    public string Candidate { get; set; }
}