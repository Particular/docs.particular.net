namespace Testing_8.UpgradeGuides._7to8
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    class FluentSagaTests
    {
        #region 7to8-testsaga
        [Test]
        public async Task TestSagaFluent()
        {
            //Test.Saga<MySaga>()
            var sagaData = new MySaga.SagaData
            {
                Originator = "Originator"
            };
            var saga = new MySaga
            {
                Data = sagaData
            };

            // .When(
            //   sagaIsInvoked: (saga, context) =>
            //   {
            //     return saga.Handle(new StartsSaga(), context);
            //   })
            var message = new StartsSaga();
            var messageHandlerContext = new TestableMessageHandlerContext();

            await saga.Handle(message, messageHandlerContext);

            // .ExpectReplyToOriginator<MyResponse>()
            var repliedMessage = messageHandlerContext.RepliedMessages.SingleOrDefault();
            Assert.IsNotNull(repliedMessage);
            Assert.AreEqual("Originator", repliedMessage.Options.GetDestination());
            Assert.IsNotNull(repliedMessage.Message<MyResponse>());

            //    .ExpectTimeoutToBeSetIn<StartsSaga>(
            //        check: (state, span) =>
            //        {
            //            return span == TimeSpan.FromDays(7);
            //        })
            var timeout = messageHandlerContext.TimeoutMessages.SingleOrDefault();
            Assert.IsNotNull(timeout);
            Assert.AreEqual(TimeSpan.FromDays(7), timeout.Within);
            var timeoutMessage = timeout.Message as StartsSaga;
            Assert.IsNotNull(timeoutMessage);

            //    .ExpectPublish<MyEvent>()
            Assert.IsTrue(messageHandlerContext.PublishedMessages
                .Any(x => x.Message is MyEvent));

            //    .ExpectSend<MyCommand>()
            Assert.IsNotNull(messageHandlerContext.SentMessages
                .SingleOrDefault(x => x.Message is MyCommand));

            //    .WhenSagaTimesOut()
            var timeoutMessageHandlerContext = new TestableMessageHandlerContext();
            await saga.Timeout(timeoutMessage, timeoutMessageHandlerContext);
            
            //    .ExpectPublish<MyOtherEvent>()
            var timeoutPublish = timeoutMessageHandlerContext.PublishedMessages.SingleOrDefault();
            Assert.IsNotNull(timeoutPublish?.Message<MyOtherEvent>());
            
            //    .ExpectSagaCompleted();
            Assert.IsTrue(saga.Completed);
        }
        #endregion
    }

    class StartsSaga : IMessage {}
    class MyResponse : IMessage {}
    class MyEvent : IEvent {}
    class MyCommand : ICommand {}
    class MyOtherEvent : IEvent {}

    class MySaga : NServiceBus.Saga<MySaga.SagaData>, IAmStartedByMessages<StartsSaga>, IHandleTimeouts<StartsSaga>
    {
        internal class SagaData : ContainSagaData
        {

        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
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
