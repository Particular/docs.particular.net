using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace CandidateVoteCount
{
    public class CandidateVotes : Saga<CandidateVoteData>,
        IAmStartedByMessages<PlaceVote>,
        IHandleTimeouts<CloseVoting>
    {
        public async Task Handle(PlaceVote message, IMessageHandlerContext context)
        {
            if (!Data.Started)
            {
                // This won't work until we have Core PR #4474 
                await RequestTimeout<CloseVoting>(context, DateTime.UtcNow.AddMinutes(1));
                Data.Candidate = message.Candidate;
                Data.Started = true;
            }
            Data.Count++;

            await context.Send(new TrackZipCode()
            {
                ZipCode = message.ZipCode
            });
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CandidateVoteData> mapper)
        {
            mapper.ConfigureMapping<PlaceVote>(m => m.Candidate).ToSaga(s => s.Candidate);
        }

        public async Task Timeout(CloseVoting state, IMessageHandlerContext context)
        {
            await context.SendLocal(new ReportVotes()
            {
                Candidate = Data.Candidate,
                NumberOfVotes = Data.Count
            });

            MarkAsComplete();
        }
    }
}