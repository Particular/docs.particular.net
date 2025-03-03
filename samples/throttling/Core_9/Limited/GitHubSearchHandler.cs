using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Octokit;
#region SearchHandler

public class GitHubSearchHandler(ILogger<GitHubSearchHandler> logger) :
    IHandleMessages<SearchGitHub>
{

    // use anonymous access which has strict rate limitations
    GitHubClient GitHubClient = new GitHubClient(new ProductHeaderValue("ThroughputThrottling"));

    public async Task Handle(SearchGitHub message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received search request for branch '{message.Branch}' on '{message.Owner}/{message.Repository}'");

        var result = await GitHubClient.Repository.Branch.Get(message.Owner, message.Repository, "master");
        logger.LogInformation($"Found commit '{result.Commit.Sha}' for branch '{message.Branch}'. Replying.");
        var response = new SearchResponse
        {
            Branch = message.Branch,
            CommitSha = result.Commit.Sha
        };
        await context.Reply(response);
    }
}
#endregion