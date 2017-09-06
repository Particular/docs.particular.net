using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;

[ServiceFabricSaga(CollectionName = "zipcode-votes")]
public class ZipCodeVotesSaga :
    Saga<ZipCodeVotesSaga.ZipCodeVoteData>,
    IAmStartedByMessages<TrackZipCode>,
    IHandleTimeouts<CloseVoting>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ZipCodeVoteData> mapper)
    {
        mapper.ConfigureMapping<TrackZipCode>(m => m.ZipCode)
            .ToSaga(s => s.ZipCode);
    }

    public async Task Handle(TrackZipCode message, IMessageHandlerContext context)
    {
        if (!Data.Started)
        {
            await RequestTimeout<CloseVoting>(context, DateTime.UtcNow.AddMinutes(1))
                .ConfigureAwait(false);
            Data.ZipCode = message.ZipCode;
            Data.Started = true;
        }

        Data.Count++;

        var trackZipCodeReply = new TrackZipCodeReply
        {
            ZipCode = Data.ZipCode,
            CurrentCount = Data.Count
        };
        await context.Reply(trackZipCodeReply)
            .ConfigureAwait(false);
    }

    public Task Timeout(CloseVoting state, IMessageHandlerContext context)
    {
        MarkAsComplete();

        var reportZipCode = new ReportZipCode
        {
            ZipCode = Data.ZipCode,
            NumberOfVotes = Data.Count
        };
        return context.SendLocal(reportZipCode);
    }

    public class ZipCodeVoteData :
        ContainSagaData
    {
        public string ZipCode { get; set; }
        public int Count { get; set; }
        public bool Started { get; set; }
    }
}