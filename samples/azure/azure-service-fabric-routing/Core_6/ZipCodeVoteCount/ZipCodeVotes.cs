using System;
using System.Threading.Tasks;
using NServiceBus;

namespace ZipCodeVoteCount
{
    using Contracts;

    public  class ZipCodeVotes : Saga<ZipCodeVoteData>,
        IAmStartedByMessages<TrackZipCode>,
        IHandleTimeouts<CloseVoting>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ZipCodeVoteData> mapper)
        {
            mapper.ConfigureMapping<TrackZipCode>(m => m.ZipCode).ToSaga(s => s.ZipCode);
        }

        public async Task Handle(TrackZipCode message, IMessageHandlerContext context)
        {
            if (!Data.Started)
            {
                await RequestTimeout<CloseVoting>(context, DateTime.UtcNow.AddMinutes(1)).ConfigureAwait(false);
                Data.ZipCode = message.ZipCode;
                Data.Started = true;
            }

            Data.Count++;

            await context.Reply(new TrackZipCodeReply
            {
                ZipCode = Data.ZipCode,
                CurrentCount = Data.Count
            }).ConfigureAwait(false);
        }

        public async Task Timeout(CloseVoting state, IMessageHandlerContext context)
        {
            await context.SendLocal(new ReportZipCode()
            {
                ZipCode = Data.ZipCode,
                NumberOfVotes = Data.Count
            }).ConfigureAwait(false);

            MarkAsComplete();
        }
    }
}