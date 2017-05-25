﻿using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Octokit;
#region SearchHandler

public class GitHubSearchHandler :
    IHandleMessages<SearchGitHub>
{
    static ILog log = LogManager.GetLogger<GitHubSearchHandler>();

    // use anonymous access which has strict rate limitations
    GitHubClient GitHubClient = new GitHubClient(new ProductHeaderValue("ThroughputThrottlingSample"));

    public async Task Handle(SearchGitHub message, IMessageHandlerContext context)
    {
        log.Info($"Received search request for '{message.SearchFor}' on {message.RepositoryOwner}/{message.Repository}...");

        var request = new SearchCodeRequest(
            message.SearchFor,
            message.RepositoryOwner,
            message.Repository);
        var result = await GitHubClient.Search.SearchCode(request)
            .ConfigureAwait(false);
        log.Info($"Found {result.TotalCount} results for {message.SearchFor}. Replying.");
        var response = new SearchResponse
        {
            SearchedFor = message.SearchFor,
            TotalCount = result.TotalCount
        };
        await context.Reply(response)
            .ConfigureAwait(false);
    }
}
#endregion