namespace Testing_10.UpgradeGuides._10to11;

using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class NegativeAdvanceTime
{
    #region 10to11-negative-advance-time
    [Test]
    public async Task AdvanceTimeWithNegativeTimeSpan()
    {
        var startTime = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        var testableSaga = new TestableSaga<EmptySaga, EmptySagaData>(initialCurrentTime: startTime);

        await testableSaga.AdvanceTime(TimeSpan.FromMinutes(-5));

        Assert.That(testableSaga.CurrentTime, Is.EqualTo(startTime.AddMinutes(-5)));
    }
    #endregion

    public class EmptySaga : Saga<EmptySagaData>, IAmStartedByMessages<Start>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<EmptySagaData> mapper) =>
            mapper.MapSaga(saga => saga.CorrelationId)
                .ToMessage<Start>(msg => msg.CorrelationId);

        public Task Handle(Start message, IMessageHandlerContext context) => Task.CompletedTask;
    }

    public class EmptySagaData : ContainSagaData
    {
        public string CorrelationId { get; set; }
    }

    public class Start : ICommand
    {
        public string CorrelationId { get; set; }
    }
}
