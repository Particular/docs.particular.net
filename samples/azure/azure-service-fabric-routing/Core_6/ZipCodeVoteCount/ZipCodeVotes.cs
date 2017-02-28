using System;
using System.Threading.Tasks;
using NServiceBus;

public class ZipCodeVotes :
    Saga<ZipCodeVotes.ZipCodeVoteData>,
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

    public async Task Timeout(CloseVoting state, IMessageHandlerContext context)
    {
        var reportZipCode = new ReportZipCode
        {
            ZipCode = Data.ZipCode,
            NumberOfVotes = Data.Count
        };
        await context.SendLocal(reportZipCode)
            .ConfigureAwait(false);

        MarkAsComplete();
    }

    public class ZipCodeVoteData :
        ContainSagaData
    {
        public string ZipCode { get; set; }
        public int Count { get; set; }
        public bool Started { get; set; }
    }
}