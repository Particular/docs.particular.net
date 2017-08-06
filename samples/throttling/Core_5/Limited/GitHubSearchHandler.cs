using NServiceBus;
using NServiceBus.Logging;
using Octokit;
#region SearchHandler
public class GitHubSearchHandler :
    IHandleMessages<SearchGitHub>
{
    IBus bus;
    static ILog log = LogManager.GetLogger<GitHubSearchHandler>();

    // use anonymous access which has strict rate limitations
    GitHubClient GitHubClient = new GitHubClient(new ProductHeaderValue("ThroughputThrottling"));

    public GitHubSearchHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(SearchGitHub message)
    {
        log.Info($"Received search for '{message.SearchFor}' on {message.Owner}/{message.Repository}");

        var request = new SearchCodeRequest(
            message.SearchFor,
            message.Owner,
            message.Repository);
        var result = GitHubClient.Search.SearchCode(request).GetAwaiter().GetResult();
        log.Info($"Found {result.TotalCount} results for {message.SearchFor}. Replying.");
        var response = new SearchResponse
        {
            SearchedFor = message.SearchFor,
            TotalCount = result.TotalCount
        };
        bus.Reply(response);
    }
}
#endregion