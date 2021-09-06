namespace Testing_7.UpgradeGuides._7to8
{
    using System;
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
        public void TestSagaFluent()
        {
            Test.Saga<MySaga>()
                .ExpectReplyToOriginator<MyResponse>()
                .ExpectTimeoutToBeSetIn<StartsSaga>(
                    check: (state, span) =>
                    {
                        return span == TimeSpan.FromDays(7);
                    })
                .ExpectPublish<MyEvent>()
                .ExpectSend<MyCommand>()
                .When(
                    sagaIsInvoked: (saga, context) =>
                    {
                        return saga.Handle(new StartsSaga(), context);
                    })
                .ExpectPublish<MyOtherEvent>()
                .WhenSagaTimesOut()
                .ExpectSagaCompleted();
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
