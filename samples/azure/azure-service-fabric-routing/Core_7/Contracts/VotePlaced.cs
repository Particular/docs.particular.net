using NServiceBus;

public class VotePlaced :
    IEvent
{
    public string Candidate { get; set; }
    public string ZipCode { get; set; }
}