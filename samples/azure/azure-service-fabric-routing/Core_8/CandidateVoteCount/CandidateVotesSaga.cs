
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;

[ServiceFabricSaga(CollectionName = "candidate-votes")]
public class CandidateVotesSaga :
    Saga<CandidateVotesSaga.CandidateVoteData>,
    IAmStartedByMessages<VotePlaced>,
    IHandleMessages<CloseElection>,
    IHandleMessages<TrackZipCodeReply>
{
    private readonly ILogger<CandidateVotesSaga> logger;

    public CandidateVotesSaga(ILogger<CandidateVotesSaga> logger)
    {
        this.logger = logger;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CandidateVoteData> mapper)
    {
        mapper.MapSaga(s => s.Candidate)
            .ToMessage<VotePlaced>(m => m.Candidate)
            .ToMessage<CloseElection>(m => m.Candidate);
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
        logger.LogInformation("##### CandidateVote saga for {Candidate} got reply for zip code '{ZipCode}' tracking with current count of {CurrentCount}", Data.Candidate, message.ZipCode, message.CurrentCount);
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