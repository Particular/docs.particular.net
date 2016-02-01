using NServiceBus;

namespace ThroughputThrottlingDemo
{
    public class SearchGitHub : IMessage
    {
        public string SearchFor { get; set; }
        public string Repository { get; set; }
        public string RepositoryOwner { get; set; }
    }
}