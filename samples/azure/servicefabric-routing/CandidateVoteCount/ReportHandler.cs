using System.Fabric;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace CandidateVoteCount
{
    public class ReportHandler : IHandleMessages<ReportVotes>
    {
        public StatefulServiceContext ServiceContext { get; set; }

        public Task Handle(ReportVotes message, IMessageHandlerContext context)
        {
            ServiceEventSource.Current.ServiceMessage(ServiceContext, $"Closing voting, {message.Candidate} got {message.NumberOfVotes} votes ");

            return Task.FromResult(true);
        }
    }
}