using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
#region GitHubSearchResponseHandler
public class GitHubSearchResponseHandler :
    IHandleMessages<SearchResponse>
{
    static ILog log = LogManager.GetLogger<GitHubSearchResponseHandler>();

    public Task Handle(SearchResponse message, IMessageHandlerContext context)
    {
        log.Info($"Found {message.TotalCount} results for {message.SearchedFor}.");
        return Task.FromResult(0);
    }
}
#endregion