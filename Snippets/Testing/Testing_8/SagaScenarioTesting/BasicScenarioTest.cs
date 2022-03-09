namespace Testing.SagaScenarioTesting
{
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
            var testableSaga = new TestableSaga<ShippingPolicy, ShippingPolicyData>();
            var orderId = Guid.NewGuid().ToString().Substring(0, 8);

            var placeResult = await testableSaga.Handle(new OrderPlaced { OrderId = orderId });
            var billResult = await testableSaga.Handle(new OrderBilled { OrderId = orderId });

            Assert.That(placeResult.Completed, Is.False);
            Assert.That(billResult.Completed, Is.False);

            // Snapshots of data are still assertable even after multiple operations have occurred.
            Assert.That(placeResult.SagaDataSnapshot.OrderId, Is.EqualTo(orderId));
            Assert.That(placeResult.SagaDataSnapshot.Placed, Is.True);
            Assert.That(placeResult.SagaDataSnapshot.Billed, Is.False);

            var noResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(10));
            Assert.That(noResults.Length, Is.EqualTo(0));

            var timeoutResults = await testableSaga.AdvanceTime(TimeSpan.FromHours(1));

            Assert.That(timeoutResults.Length, Is.EqualTo(1));

            var shipped = timeoutResults.First().FindPublishedMessage<OrderShipped>();
            Assert.That(shipped.OrderId == "abc");
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
}