using Microsoft.Extensions.Time.Testing;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class TimeProviderUsage
{
    [Test]
    public void CtorTimeProvider()
    {
        #region TestableSagaCtorTimeProvider
        var testableSaga = new TestableSaga<ShippingPolicy, ShippingPolicyData>(
            sagaFactory: timeProvider => new ShippingPolicy(timeProvider, new ShippingCalendar(timeProvider)));
        #endregion

        Assert.That(testableSaga.TimeProvider, Is.Not.Null);
    }

    [Test]
    public void CtorSharedTimeProvider()
    {
        #region TestableSagaCtorSharedTimeProvider
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero));
        var shippingCalendar = new ShippingCalendar(timeProvider);

        var testableSaga = new TestableSaga<ShippingPolicy, ShippingPolicyData>(
            sagaFactory: _ => new ShippingPolicy(timeProvider, shippingCalendar),
            timeProvider: timeProvider);
        #endregion

        Assert.That(testableSaga.TimeProvider, Is.SameAs(timeProvider));
    }

    [Test]
    public async Task AdvanceTimeProviderDirectly()
    {
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero));
        var testableSaga = new TestableSaga<ShippingPolicy, ShippingPolicyData>(
            sagaFactory: _ => new ShippingPolicy(timeProvider, new ShippingCalendar(timeProvider)),
            timeProvider: timeProvider);

        await testableSaga.Handle(new OrderPlaced { OrderId = "abc" });
        await testableSaga.Handle(new OrderBilled { OrderId = "abc" });

        #region TestableSagaTimeProviderDirect
        // The same value as testableSaga.CurrentTime
        DateTimeOffset now = testableSaga.TimeProvider.GetUtcNow();

        testableSaga.TimeProvider.Advance(TimeSpan.FromHours(1));

        // Timeouts that became due are handled by the next AdvanceTime call
        var timeoutResults = await testableSaga.AdvanceTime(TimeSpan.Zero);
        #endregion

        Assert.Multiple(() =>
        {
            Assert.That(timeoutResults, Has.Length.EqualTo(1));
            Assert.That(timeoutResults.Single().SagaDataSnapshot.ShippedAt, Is.EqualTo(now.AddHours(1)));
        });
    }

    [Test]
    public async Task TimeoutHandlerSeesDueTime()
    {
        var startTime = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var testableSaga = new TestableSaga<ShippingPolicy, ShippingPolicyData>(
            sagaFactory: timeProvider => new ShippingPolicy(timeProvider, new ShippingCalendar(timeProvider)),
            timeProvider: new FakeTimeProvider(startTime));

        await testableSaga.Handle(new OrderPlaced { OrderId = "abc" });
        await testableSaga.Handle(new OrderBilled { OrderId = "abc" });

        #region TestableSagaTimeProviderDueTime
        var timeoutResults = await testableSaga.AdvanceTime(TimeSpan.FromHours(1));

        // The handler saw the due time of the timeout, not the end of the advanced period
        DateTimeOffset shippedAt = timeoutResults.Single().SagaDataSnapshot.ShippedAt;

        DateTime newCurrentTime = testableSaga.CurrentTime;
        #endregion

        Assert.Multiple(() =>
        {
            Assert.That(shippedAt, Is.EqualTo(startTime.AddMinutes(15)));
            Assert.That(newCurrentTime, Is.EqualTo(startTime.AddHours(1).UtcDateTime));
        });
    }

    public class ShippingCalendar(TimeProvider timeProvider)
    {
        public bool IsWeekend() => timeProvider.GetUtcNow().DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    public class ShippingPolicy(TimeProvider timeProvider, ShippingCalendar shippingCalendar) : Saga<ShippingPolicyData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>,
        IHandleTimeouts<ShippingDelay>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.MapSaga(saga => saga.OrderId)
                .ToMessage<OrderPlaced>(msg => msg.OrderId)
                .ToMessage<OrderBilled>(msg => msg.OrderId);
        }

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Data.Placed = true;
            return TimeToShip(context);
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Data.Billed = true;
            return TimeToShip(context);
        }

        Task TimeToShip(IMessageHandlerContext context) =>
            Data.Placed && Data.Billed
                ? RequestTimeout<ShippingDelay>(context, shippingCalendar.IsWeekend() ? TimeSpan.FromDays(2) : TimeSpan.FromMinutes(15))
                : Task.CompletedTask;

        public Task Timeout(ShippingDelay state, IMessageHandlerContext context)
        {
            Data.ShippedAt = timeProvider.GetUtcNow();
            MarkAsComplete();
            return Task.CompletedTask;
        }
    }

    public class ShippingPolicyData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool Placed { get; set; }
        public bool Billed { get; set; }
        public DateTimeOffset ShippedAt { get; set; }
    }

    public class OrderPlaced : IEvent
    {
        public string OrderId { get; set; }
    }

    public class OrderBilled : IEvent
    {
        public string OrderId { get; set; }
    }

    public class ShippingDelay;
}
