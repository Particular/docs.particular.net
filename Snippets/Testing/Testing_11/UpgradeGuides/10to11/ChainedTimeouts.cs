namespace Testing_11.UpgradeGuides._10to11;

using System;
using System.Linq;
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
            Assert.That(firstResults, Has.Length.EqualTo(6));
            Assert.That(secondResults, Is.Empty);
            Assert.That(thirdResults, Is.Empty);
        });
    }
    #endregion

    #region 10to11-chained-timeouts-control
    [Test]
    public async Task ReminderSagaOneReminderAtATime()
    {
        var testableSaga = new TestableSaga<ReminderSaga, ReminderSagaData>();
        await testableSaga.Handle(new StartReminders { CorrelationId = "abc" });

        // Advancing by the reminder interval handles one reminder per call
        var firstResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(10));
        var secondResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(10));

        Assert.Multiple(() =>
        {
            Assert.That(firstResults.Single().SagaDataSnapshot.RemindersSent, Is.EqualTo(1));
            Assert.That(secondResults.Single().SagaDataSnapshot.RemindersSent, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task ReminderSagaInspectEachReminder()
    {
        var testableSaga = new TestableSaga<ReminderSaga, ReminderSagaData>();
        await testableSaga.Handle(new StartReminders { CorrelationId = "abc" });

        var results = await testableSaga.AdvanceTime(TimeSpan.FromHours(1));

        // Each result holds a copy of the saga data after that reminder
        Assert.That(
            results.Select(result => result.SagaDataSnapshot.RemindersSent),
            Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
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
