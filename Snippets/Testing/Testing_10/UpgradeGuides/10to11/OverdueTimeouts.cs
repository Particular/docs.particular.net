namespace Testing_10.UpgradeGuides._10to11;

using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class OverdueTimeouts
{
    #region 10to11-overdue-timeout
    [Test]
    public async Task FollowUpIsScheduledFromWhenTheOverdueTimeoutWasHandled()
    {
        var startTime = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        var testableSaga = new TestableSaga<ScheduledSaga, ScheduledSagaData>(initialCurrentTime: startTime);

        await testableSaga.AdvanceTime(TimeSpan.FromHours(1));
        // The timeout is due at 8:30 AM, which is already in the past
        await testableSaga.Handle(new StartAt
        {
            CorrelationId = "abc",
            TimeoutAt = startTime.AddMinutes(30)
        });

        var overdueResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(1));
        var followUpResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(1));

        Assert.Multiple(() =>
        {
            Assert.That(overdueResults, Has.Length.EqualTo(1));
            Assert.That(followUpResults, Has.Length.EqualTo(1));
        });
    }
    #endregion

    public class ScheduledSaga : Saga<ScheduledSagaData>,
        IAmStartedByMessages<StartAt>,
        IHandleTimeouts<Scheduled>,
        IHandleTimeouts<FollowUp>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ScheduledSagaData> mapper) =>
            mapper.MapSaga(saga => saga.CorrelationId)
                .ToMessage<StartAt>(msg => msg.CorrelationId);

        public Task Handle(StartAt message, IMessageHandlerContext context) =>
            RequestTimeout<Scheduled>(context, message.TimeoutAt);

        public Task Timeout(Scheduled state, IMessageHandlerContext context) =>
            RequestTimeout<FollowUp>(context, TimeSpan.FromMinutes(15));

        public Task Timeout(FollowUp state, IMessageHandlerContext context) => Task.CompletedTask;
    }

    public class ScheduledSagaData : ContainSagaData
    {
        public string CorrelationId { get; set; }
    }

    public class StartAt : ICommand
    {
        public string CorrelationId { get; set; }
        public DateTimeOffset TimeoutAt { get; set; }
    }

    public class Scheduled;

    public class FollowUp;
}
