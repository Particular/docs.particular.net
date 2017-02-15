using System.Threading.Tasks;
using NServiceBus;

namespace CandidateVoteCount
{
    using Contracts;

    public class CandidateVotes : Saga<CandidateVoteData>,
        IAmStartedByMessages<VotePlaced>,
        IHandleMessages<CloseElection>
    {
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

            return context.Send(new TrackZipCode
            {
                ZipCode = message.ZipCode
            });
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
    }
}