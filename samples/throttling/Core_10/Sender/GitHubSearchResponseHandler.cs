using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
#region GitHubSearchResponseHandler
public class GitHubSearchResponseHandler(ILogger<GitHubSearchResponseHandler> logger) :
    IHandleMessages<SearchResponse>
{
    public Task Handle(SearchResponse message, IMessageHandlerContext context)
    {
        logger.LogInformation("Found commit '{CommitSha}' for branch '{Branch}.'", message.CommitSha, message.Branch);
        return Task.CompletedTask;
    }
}
#endregion