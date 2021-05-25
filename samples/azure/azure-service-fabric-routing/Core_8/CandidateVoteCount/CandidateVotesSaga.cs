using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;

[ServiceFabricSaga(CollectionName = "candidate-votes")]
public class CandidateVotesSaga :
    Saga<CandidateVotesSaga.CandidateVoteData>,
        IAmStartedByMessages<VotePlaced>,
        IHandleMessages<CloseElection>,
        IHandleMessages<TrackZipCodeReply>
{

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CandidateVoteData> mapper)
    {
        mapper.ConfigureMapping<VotePlaced>(m => m.Candidate)
            .ToSaga(s => s.Candidate);
        mapper.ConfigureMapping<CloseElection>(m => m.Candidate)
            .ToSaga(s => s.Candidate);
    }

    public Task Handle(VotePlaced message, IMessageHandlerContext context)
    {
        if (!Data.Started)
        {
            Data.Candidate = message.Candidate;
            Data.Started = true;
        }
        Data.Count++;

        var trackZipCode = new TrackZipCode
        {
            ZipCode = message.ZipCode
        };
        return context.Send(trackZipCode);
    }

    public Task Handle(CloseElection message, IMessageHandlerContext context)
    {
        var reportVotes = new ReportVotes
        {
            Candidate = Data.Candidate,
            NumberOfVotes = Data.Count
        };

        MarkAsComplete();
        return context.SendLocal(reportVotes);
    }

    public Task Handle(TrackZipCodeReply message, IMessageHandlerContext context)
    {
        Logger.Log($"##### CandidateVote saga for {Data.Candidate} got reply for zip code '{message.ZipCode}' tracking with current count of {message.CurrentCount}");
        return Task.CompletedTask;
    }

    public class CandidateVoteData :
        ContainSagaData
    {
        public bool Started { get; set; }
        public string Candidate { get; set; }
        public int Count { get; set; }
    }
}