namespace Testing_10.UpgradeGuides._10to11;

using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class ChainedTimeouts
{
    #region 10to11-chained-timeouts
    [Test]
    public async Task ReminderSagaRequestsNextReminderFromEachReminder()
    {
        var testableSaga = new TestableSaga<ReminderSaga, ReminderSagaData>();
        await testableSaga.Handle(new StartReminders { CorrelationId = "abc" });

        var firstResults = await testableSaga.AdvanceTime(TimeSpan.FromHours(1));
        var secondResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(5));
        var thirdResults = await testableSaga.AdvanceTime(TimeSpan.Zero);

        Assert.Multiple(() =>
        {
            Assert.That(firstResults, Has.Length.EqualTo(1));
            Assert.That(secondResults, Has.Length.EqualTo(1));
            Assert.That(thirdResults, Has.Length.EqualTo(1));
        });
    }
    #endregion

    public class ReminderSaga : Saga<ReminderSagaData>,
        IAmStartedByMessages<StartReminders>,
        IHandleTimeouts<Reminder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ReminderSagaData> mapper) =>
            mapper.MapSaga(saga => saga.CorrelationId)
                .ToMessage<StartReminders>(msg => msg.CorrelationId);

        public Task Handle(StartReminders message, IMessageHandlerContext context) =>
            RequestTimeout<Reminder>(context, TimeSpan.FromMinutes(10));

        public Task Timeout(Reminder state, IMessageHandlerContext context)
        {
            Data.RemindersSent++;
            return RequestTimeout<Reminder>(context, TimeSpan.FromMinutes(10));
        }
    }

    public class ReminderSagaData : ContainSagaData
    {
        public string CorrelationId { get; set; }
        public int RemindersSent { get; set; }
    }

    public class StartReminders : ICommand
    {
        public string CorrelationId { get; set; }
    }

    public class Reminder;
}
