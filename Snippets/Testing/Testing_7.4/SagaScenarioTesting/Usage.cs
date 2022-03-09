namespace Testing.SagaScenarioTesting
{
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class Usage
    {
        [Test]
        public async Task SampleSagaScenarioTest()
        {
            // Create the testable saga
            var testableSaga = new TestableSaga<MySaga, MyData>();

            // Create input messages
            var orderId = Guid.NewGuid().ToString().Substring(0, 8);
            var orderPlaced = new MyCommand { CorrelationId = orderId };
            var orderBilled = new OrderBilled { CorrelationId = orderId };

            // Process OrderPlaced and make assertions on the result
            var placeResult = await testableSaga.Handle(orderPlaced);
            Assert.That(placeResult.Completed, Is.False);
            Assert.That(placeResult.FindPublishedMessage<OrderShipped>(), Is.Null);
            Assert.That(placeResult.FindTimeoutMessage<MyTimeout>(), Is.Null);

            // Process OrderBilled and make assertions on the result
            var billResult = await testableSaga.Handle(orderBilled);
            Assert.That(billResult.Completed, Is.False);
            Assert.That(billResult.FindPublishedMessage<OrderShipped>(), Is.Null);
            Assert.That(billResult.FindTimeoutMessage<MyTimeout>(), Is.Not.Null);

            // Each result includes a snapshot of saga data after each message.
            // Snapshots can be asserted even after multiple operations have occurred.
            Assert.That(placeResult.SagaDataSnapshot.OrderId, Is.EqualTo(orderId));
            Assert.That(placeResult.SagaDataSnapshot.Placed, Is.True);
            Assert.That(placeResult.SagaDataSnapshot.Billed, Is.False);

            // Timeouts are stored and can be played by advancing time
            var noResults = await testableSaga.AdvanceTime(TimeSpan.FromMinutes(10));
            // But that wasn't long enough
            Assert.That(noResults.Length, Is.EqualTo(0));

            // Advance time more to get the timeout to fire
            var timeoutResults = await testableSaga.AdvanceTime(TimeSpan.FromHours(1));
            Assert.That(timeoutResults.Length, Is.EqualTo(1));
            var shipped = timeoutResults.First().FindPublishedMessage<OrderShipped>();
            Assert.That(shipped.CorrelationId == orderId);
        }

        public void Ctor()
        {
            #region TestableSagaCtor
            // For testing: public class MySaga : Saga<MyData>
            var testableSaga = new TestableSaga<MySaga, MyData>();
            #endregion
        }

        public void CtorFactory()
        {
            #region TestableSagaCtorFactory
            var testableSaga = new TestableSaga<MySaga, MyData>(
                sagaFactory: () => new MySaga(new InjectedService()));
            #endregion
        }

        public void CtorTime()
        {
            #region TestableSagaCtorTime
            var testableSaga = new TestableSaga<MySaga, MyData>(
                initialCurrentTime: new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc));
            #endregion
        }

        public async Task Defaults(TestableSaga<MySaga, MyData> testableSaga)
        {
            #region TestableSagaSimpleHandle
            var handleResult = await testableSaga.Handle(new MyCommand());
            #endregion

            var timeSpanToAdvance = TimeSpan.FromHours(1);

            #region TestableSagaAdvanceTime
            // Returns HandleResult[]
            var timeoutResults = await testableSaga.AdvanceTime(timeSpanToAdvance);
            #endregion
        }

        public async Task AllParams(TestableSaga<MySaga, MyData> testableSaga)
        {
            #region TestableSagaHandleParams
            var customHandlerContext = new TestableMessageHandlerContext();
            var customHeaders = new Dictionary<string, string>();

            var handleResult = await testableSaga.Handle(new MyCommand(), context: customHandlerContext, messageHeaders: customHeaders);
            #endregion

            var timeSpanToAdvance = TimeSpan.FromHours(1);

            #region TestableSagaAdvanceTimeParams
            // Returns HandleResult[]
            var timeoutResults = await testableSaga.AdvanceTime(timeSpanToAdvance,
                provideContext: timeoutDetails =>
                {
                    // Provide custom context based on timeout details
                    return new TestableMessageHandlerContext();
                });
            #endregion
        }

        public class InjectedService { }

        public class MySaga : NServiceBus.Saga<MyData>,
            IAmStartedByMessages<MyCommand>,
            IAmStartedByMessages<OrderBilled>,
            IHandleTimeouts<MyTimeout>
        {
            public MySaga() { }
            public MySaga(InjectedService service) { }

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MyData> mapper)
            {
                mapper.MapSaga(saga => saga.OrderId)
                    .ToMessage<MyCommand>(msg => msg.CorrelationId)
                    .ToMessage<OrderBilled>(msg => msg.CorrelationId);
            }

            public Task Handle(MyCommand message, IMessageHandlerContext context)
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
                    await RequestTimeout<MyTimeout>(context, TimeSpan.FromMinutes(15));
                }
            }

            public async Task Timeout(MyTimeout state, IMessageHandlerContext context)
            {
                await context.Publish(new OrderShipped { CorrelationId = Data.OrderId });
                MarkAsComplete();
            }
        }

        public class MyData : ContainSagaData
        {
            public string OrderId { get; set; }
            public bool Placed { get; set; }
            public bool Billed { get; set; }
        }

        public class MyCommand : ICommand
        {
            public string CorrelationId { get; set; }
        }

        public class OrderBilled : IEvent
        {
            public string CorrelationId { get; set; }
        }

        public class OrderShipped : IEvent
        {
            public string CorrelationId { get; set; }
        }

        public class MyTimeout { }
    }
}