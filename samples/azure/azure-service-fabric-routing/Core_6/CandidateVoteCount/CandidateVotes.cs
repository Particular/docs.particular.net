using System.Threading.Tasks;
using NServiceBus;

namespace CandidateVoteCount
{
    using System.Fabric;
    using Contracts;

    public class CandidateVotes : Saga<CandidateVoteData>,
        IAmStartedByMessages<VotePlaced>,
        IHandleMessages<CloseElection>,
        IHandleMessages<TrackZipCodeReply>
    {
        private readonly StatefulServiceContext serviceContext;

        public CandidateVotes(StatefulServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CandidateVoteData> mapper)
        {
            mapper.ConfigureMapping<VotePlaced>(m => m.Candidate).ToSaga(s => s.Candidate);
            mapper.ConfigureMapping<CloseElection>(m => m.Candidate).ToSaga(s => s.Candidate);
        }

        public Task Handle(VotePlaced message, IMessageHandlerContext context)
        {
            if (!Data.Started)
            {
                Data.Candidate = message.Candidate;
                Data.Started = true;
            }
            Data.Count++;

            var sendOptions = new SendOptions();
            sendOptions.RouteReplyToThisInstance();

            return context.Send(new TrackZipCode
            {
                ZipCode = message.ZipCode
            }, sendOptions);
        }

        public async Task Handle(CloseElection message, IMessageHandlerContext context)
        {
            await context.SendLocal(new ReportVotes
            {
                Candidate = Data.Candidate,
                NumberOfVotes = Data.Count
            }).ConfigureAwait(false);

            MarkAsComplete();
        }

        public Task Handle(TrackZipCodeReply message, IMessageHandlerContext context)
        {
            ServiceEventSource.Current.ServiceMessage(serviceContext, $"##### CandidateVote saga for {Data.Candidate} got reply for zip code '{message.ZipCode}' tracking with current count of {message.CurrentCount}");
            return Task.FromResult(0);
        }
    }
}