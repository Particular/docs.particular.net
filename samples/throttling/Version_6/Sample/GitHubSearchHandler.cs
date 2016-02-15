using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Octokit;

public class GitHubSearchHandler : IHandleMessages<SearchGitHub>
{
    static ILog log = LogManager.GetLogger<GitHubSearchHandler> ();

    // use anonymous access which has strict rate limitations
    GitHubClient GitHubClient = new GitHubClient(new ProductHeaderValue("ThroughputThrottlingSample"));

    public async Task Handle(SearchGitHub message, IMessageHandlerContext context)
    {
        log.Info($"Received search request for \"{message.SearchFor}\" on {message.RepositoryOwner}/{message.Repository}...");

        SearchCodeRequest request = new SearchCodeRequest(
            message.SearchFor,
            message.RepositoryOwner,
            message.Repository);
        SearchCodeResult result = await GitHubClient.Search.SearchCode(request);

        log.Info($"Found {result.TotalCount} results for {message.SearchFor}.");
    }
}