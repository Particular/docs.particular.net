using System.Threading.Tasks;
using NServiceBus;

public class CandidateVotes : Saga<CandidateVotes.CandidateVoteData>,
        IAmStartedByMessages<VotePlaced>,
        IHandleMessages<CloseElection>,
        IHandleMessages<TrackZipCodeReply>
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

    public Task Handle(TrackZipCodeReply message, IMessageHandlerContext context)
    {
        Logger.Log($"##### CandidateVote saga for {Data.Candidate} got reply for zip code '{message.ZipCode}' tracking with current count of {message.CurrentCount}");

        return Task.FromResult(0);
    }

    public class CandidateVoteData : ContainSagaData
    {
        public bool Started { get; set; }
        public string Candidate { get; set; }
        public int Count { get; set; }
    }
}