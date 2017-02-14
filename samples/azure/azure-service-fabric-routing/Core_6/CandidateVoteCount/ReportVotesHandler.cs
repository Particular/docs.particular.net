using System.Fabric;
using System.Threading.Tasks;
using NServiceBus;

namespace CandidateVoteCount
{
    using Contracts;

    public class ReportVotesHandler : IHandleMessages<ReportVotes>
    {
        public StatefulServiceContext ServiceContext { get; set; }

        public Task Handle(ReportVotes message, IMessageHandlerContext context)
        {
            ServiceEventSource.Current.ServiceMessage(ServiceContext, $"Closing voting, {message.Candidate} got {message.NumberOfVotes} votes ");

            return Task.FromResult(true);
        }
    }
}