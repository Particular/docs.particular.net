using System.Fabric;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

namespace CandidateVoteCount
{
    using Contracts;

    public class ReportVotesHandler : IHandleMessages<ReportVotes>
    {
        public Task Handle(ReportVotes message, IMessageHandlerContext context)
        {
            Logger.Log($"Closing voting, {message.Candidate} got {message.NumberOfVotes} votes ");

            return Task.FromResult(true);
        }
    }
}