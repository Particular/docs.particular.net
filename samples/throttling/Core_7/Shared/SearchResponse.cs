using NServiceBus;

public class SearchResponse :
    IMessage
{
    public string SearchedFor { get; set; }
    public int TotalCount { get; set; }
}