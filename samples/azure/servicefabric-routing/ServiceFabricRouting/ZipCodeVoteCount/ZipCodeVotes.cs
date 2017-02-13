using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace ZipCodeVoteCount
{
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
                await RequestTimeout<CloseVoting>(context, DateTime.UtcNow.AddMinutes(1));
                Data.ZipCode = message.ZipCode;
                Data.Started = true;
            }
            Data.Count++;
        }

        public async Task Timeout(CloseVoting state, IMessageHandlerContext context)
        {
            await context.SendLocal(new ReportZipCode()
            {
                ZipCode = Data.ZipCode,
                NumberOfVotes = Data.Count
            });
            MarkAsComplete();
        }
    }
}