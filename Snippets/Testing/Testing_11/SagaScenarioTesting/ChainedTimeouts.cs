using Microsoft.Extensions.Time.Testing;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class ChainedTimeouts
{
    [Test]
    public async Task AdvanceTimeHandlesChainedTimeouts()
    {
        var startTime = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var testableSaga = new TestableSaga<ReminderPolicy, ReminderPolicyData>(
            sagaFactory: timeProvider => new ReminderPolicy(timeProvider),
            timeProvider: new FakeTimeProvider(startTime));

        await testableSaga.Handle(new StartReminders { CorrelationId = "abc" });

        #region TestableSagaChainedTimeouts
        var timeoutResults = await testableSaga.AdvanceTime(TimeSpan.FromHours(1));

        // Reminders at 10, 20, 30, 40, 50, and 60 minutes
        Assert.That(timeoutResults, Has.Length.EqualTo(6));

        // Each reminder was handled at its own due time
        var firstReminder = timeoutResults[0].SagaDataSnapshot;
        Assert.That(firstReminder.LastReminderAt, Is.EqualTo(startTime.AddMinutes(10)));
        #endregion
    }

    [Test]
    public async Task AdvanceTimeOneReminderAtATime()
    {
        var testableSaga = new TestableSaga<ReminderPolicy, ReminderPolicyData>(
            sagaFactory: timeProvider => new ReminderPolicy(timeProvider));

        await testableSaga.Handle(new StartReminders { CorrelationId = "abc" });

        #region TestableSagaChainedTimeoutsStepwise
        // Advancing by the reminder interval handles one reminder per call
        var firstResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(10));
        Assert.That(firstResults.Single().SagaDataSnapshot.RemindersSent, Is.EqualTo(1));

        var secondResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(10));
        Assert.That(secondResults.Single().SagaDataSnapshot.RemindersSent, Is.EqualTo(2));
        #endregion
    }

    public class ReminderPolicy(TimeProvider timeProvider) : Saga<ReminderPolicyData>,
        IAmStartedByMessages<StartReminders>,
        IHandleTimeouts<Reminder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ReminderPolicyData> mapper) =>
            mapper.MapSaga(saga => saga.CorrelationId)
                .ToMessage<StartReminders>(msg => msg.CorrelationId);

        public Task Handle(StartReminders message, IMessageHandlerContext context) =>
            RequestTimeout<Reminder>(context, TimeSpan.FromMinutes(10));

        public Task Timeout(Reminder state, IMessageHandlerContext context)
        {
            Data.RemindersSent++;
            Data.LastReminderAt = timeProvider.GetUtcNow();
            return RequestTimeout<Reminder>(context, TimeSpan.FromMinutes(10));
        }
    }

    public class ReminderPolicyData : ContainSagaData
    {
        public string CorrelationId { get; set; }
        public int RemindersSent { get; set; }
        public DateTimeOffset LastReminderAt { get; set; }
    }

    public class StartReminders : ICommand
    {
        public string CorrelationId { get; set; }
    }

    public class Reminder;
}
