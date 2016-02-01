using System;
using System.Threading.Tasks;
using NServiceBus;
using Octokit;

namespace ThroughputThrottlingDemo
{
    public class GitHubSearchHandler : IHandleMessages<SearchGitHub>
    {
        // use anonymous access which has very strict rate limitations
        private static readonly GitHubClient GitHubClient = new GitHubClient(new ProductHeaderValue("ThroughputThrottlingDemo"));

        public async Task Handle(SearchGitHub message, IMessageHandlerContext context)
        {
            Console.WriteLine("received search request");

            SearchCodeResult result = await GitHubClient.Search.SearchCode(
                new SearchCodeRequest(
                    message.SearchFor, 
                    message.RepositoryOwner, 
                    message.Repository));

            Console.WriteLine($"Found {result.TotalCount} results for {message.SearchFor}.");
        }
    }
}