using NServiceBus;

public class SearchResponse : IMessage
{
    public string Branch { get; set; }

    public string CommitSha { get; set; }
}