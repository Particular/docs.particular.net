using NServiceBus;
using NServiceBus.Logging;

#region GitHubSearchResponseHandler
public class GitHubSearchResponseHandler :
    IHandleMessages<SearchResponse>
{
    static ILog log = LogManager.GetLogger<GitHubSearchResponseHandler>();

    public void Handle(SearchResponse message)
    {
        log.Info($"Found {message.TotalCount} results for {message.SearchedFor}.");
    }
}
#endregion