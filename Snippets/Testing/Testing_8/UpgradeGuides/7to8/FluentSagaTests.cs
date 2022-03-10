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
            var saga = new MySaga
            {
                Data = new MySaga.SagaData
                {
                    Originator = "Originator"
                }
            };

            // Act
            var message = new StartsSaga();
            var messageHandlerContext = new TestableMessageHandlerContext();

            await saga.Handle(message, messageHandlerContext);

            // Assert
            Assert.IsTrue(messageHandlerContext.RepliedMessages.Any(x =>
                x.Message is MyResponse &&
                x.Options.GetDestination() == "Originator"),
                "A MyResponse reply should be sent to the originator"
            );

            Assert.IsTrue(messageHandlerContext.TimeoutMessages.Any(x =>
                x.Message is StartsSaga &&
                x.Within == TimeSpan.FromDays(7)),
                "The StartsSaga message should be deferred for 7 days"
            );

            Assert.IsTrue(messageHandlerContext.PublishedMessages.Any(x =>
                x.Message is MyEvent),
                "MyEvent should be published"
            );

            Assert.IsTrue(messageHandlerContext.SentMessages.Any(x =>
                x.Message is MyCommand),
                "MyCommand should be sent"
            );
        }

        [Test]
        public async Task When_StartsSaga_Timeout_completes()
        {
            // Arrange
            var saga = new MySaga
            {
                Data = new MySaga.SagaData()
            };

            // Act
            var timeoutMessage = new StartsSaga();
            var timeoutHandlerContext = new TestableMessageHandlerContext();
            await saga.Timeout(timeoutMessage, timeoutHandlerContext);

            // Assert
            Assert.IsTrue(timeoutHandlerContext.PublishedMessages.Any(x =>
                x.Message is MyOtherEvent),
                "MyOtherEvent should be published"
            );

            Assert.IsTrue(saga.Completed, "Saga should be completed");
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
            await ReplyToOriginator(context, new MyResponse());
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
