using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class BasicScenarioTest
{
    #region BasicScenarioTest
    [Test]
    public async Task SampleSagaScenarioTest()
    {
        // Create the testable saga
        var testableSaga = new TestableSaga<ShippingPolicy, ShippingPolicyData>();

        // Create input messages
        var orderId = Guid.NewGuid().ToString().Substring(0, 8);
        var orderPlaced = new OrderPlaced { OrderId = orderId };
        var orderBilled = new OrderBilled { OrderId = orderId };

        // Process OrderPlaced and make assertions on the result
        var placeResult = await testableSaga.Handle(orderPlaced);
        Assert.Multiple(() =>
        {
            Assert.That(placeResult.Completed, Is.False);
            Assert.That(placeResult.FindPublishedMessage<OrderShipped>(), Is.Null);
            Assert.That(placeResult.FindTimeoutMessage<ShippingDelay>(), Is.Null);
        });

        // Process OrderBilled and make assertions on the result
        var billResult = await testableSaga.Handle(orderBilled);
        Assert.Multiple(() =>
        {
            Assert.That(billResult.Completed, Is.False);
            Assert.That(billResult.FindPublishedMessage<OrderShipped>(), Is.Null);
            Assert.That(billResult.FindTimeoutMessage<ShippingDelay>(), Is.Not.Null);

            // Each result includes a snapshot of saga data after each message.
            // Snapshots can be asserted even after multiple operations have occurred.
            Assert.That(placeResult.SagaDataSnapshot.OrderId, Is.EqualTo(orderId));
            Assert.That(placeResult.SagaDataSnapshot.Placed, Is.True);
            Assert.That(placeResult.SagaDataSnapshot.Billed, Is.False);
        });

        // Timeouts are stored and can be played by advancing time
        var noResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(10));
        // But that wasn't long enough
        Assert.That(noResults.Length, Is.EqualTo(0));

        // Advance time more to get the timeout to fire
        var timeoutResults = await testableSaga.AdvanceTime(TimeSpan.FromHours(1));
        Assert.That(timeoutResults, Has.Length.EqualTo(1));
        var shipped = timeoutResults.First().FindPublishedMessage<OrderShipped>();
        Assert.That(shipped.OrderId, Is.EqualTo(orderId));
    }
    #endregion



    public class ShippingPolicy : NServiceBus.Saga<ShippingPolicyData>,
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
        public async Task TimeToShip(IMessageHandlerContext context)
        {
            if (Data.Placed && Data.Billed)
            {
                await RequestTimeout<ShippingDelay>(context, TimeSpan.FromMinutes(15));
            }
        }

        public async Task Timeout(ShippingDelay state, IMessageHandlerContext context)
        {
            await context.Publish(new OrderShipped { OrderId = Data.OrderId });
            MarkAsComplete();
        }
    }

    public class ShippingPolicyData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool Placed { get; set; }
        public bool Billed { get; set; }
    }

    public class OrderPlaced : IEvent
    {
        public string OrderId { get; set; }
    }

    public class OrderBilled : IEvent
    {
        public string OrderId { get; set; }
    }

    public class OrderShipped : IEvent
    {
        public string OrderId { get; set; }
    }

    public class ShippingDelay { }
}