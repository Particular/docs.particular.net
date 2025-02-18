namespace Testing_8.UpgradeGuides._7to8
{
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Explicit]
    #region 7to8-testsaga
    [TestFixture]
    class ArrangeActAssertSagaTests
    {
        [Test]
        public async Task When_Saga_is_started_by_StartsSaga()
        {
            // Arrange
            var testableSaga = new TestableSaga<MySaga, MySaga.SagaData>();

            // Act
            var message = new StartsSaga { MyId = "some-id" };
            var startResult = await testableSaga.Handle(message);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(startResult.FindPublishedMessage<MyEvent>(), Is.Not.Null,
                    "MyEvent should be published");

                Assert.That(startResult.FindSentMessage<MyCommand>(), Is.Not.Null,
                    "MyCommand should be sent");
            });

            // Instead of asserting on timeouts placed, virtually advance time
            // and then assert on the results
            var advanceTimeResults = await testableSaga.AdvanceTime(TimeSpan.FromDays(7));

            Assert.That(advanceTimeResults, Has.Length.EqualTo(1));
            var timeoutResult = advanceTimeResults.Single();
            Assert.That(timeoutResult.Completed, Is.True);
        }
    }
    #endregion

    class StartsSaga : IMessage
    {
        public string MyId { get; set; }
    }
    class MyResponse : IMessage { }
    class MyEvent : IEvent { }
    class MyCommand : ICommand { }
    class MyOtherEvent : IEvent { }

    class MySaga : NServiceBus.Saga<MySaga.SagaData>, IAmStartedByMessages<StartsSaga>, IHandleTimeouts<StartsSaga>
    {
        internal class SagaData : ContainSagaData
        {
            public string MyId { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.MapSaga(saga => saga.MyId)
                .ToMessage<StartsSaga>(msg => msg.MyId);
        }

        public async Task Handle(StartsSaga message, IMessageHandlerContext context)
        {
            await RequestTimeout(context, TimeSpan.FromDays(7), message);
            await context.Publish<MyEvent>();
            await context.Send(new MyCommand());
        }

        public async Task Timeout(StartsSaga state, IMessageHandlerContext context)
        {
            await context.Publish<MyOtherEvent>();
            MarkAsComplete();
        }
    }
}
