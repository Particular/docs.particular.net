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
        logger.LogInformation("Received search request for branch '{Branch}' on '{Owner}/{Repository}'", message.Branch, message.Owner, message.Repository);

        var result = await GitHubClient.Repository.Branch.Get(message.Owner, message.Repository, "master");
        logger.LogInformation("Found commit '{CommitSha}' for branch '{Branch}'. Replying.", result.Commit.Sha, message.Branch);
        var response = new SearchResponse
        {
            Branch = message.Branch,
            CommitSha = result.Commit.Sha
        };
        await context.Reply(response);
    }
}
#endregion