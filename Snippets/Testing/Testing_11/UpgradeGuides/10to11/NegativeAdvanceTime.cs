namespace Testing_11.UpgradeGuides._10to11;

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
    public void AdvanceTimeWithNegativeTimeSpan()
    {
        var testableSaga = new TestableSaga<EmptySaga, EmptySagaData>();

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            async () => await testableSaga.AdvanceTime(TimeSpan.FromMinutes(-5)));
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
