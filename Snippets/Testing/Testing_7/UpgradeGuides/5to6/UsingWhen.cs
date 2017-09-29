namespace Testing_6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus.Testing;
    using NUnit.Framework;
    using Testing_6.Saga;

    [Explicit]
    [TestFixture]
    public class SagaTests
    {
        [Test]
        public void Run()
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
                #region 5to6-usingWhen

                .When(
                    sagaIsInvoked: (saga, context) =>
                    {
                        return saga.Handle(new StartsSaga(), context);
                    })
                #endregion

                .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                .ExpectSagaCompleted();
        }

        [Test]
        public void NewOverload()
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
                #region 5to6-usingNewOverload

                .When(
                    handlerSelector: saga =>
                    {
                        return saga.Handle;
                    },
                    message: new StartsSaga())
                #endregion

                .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                .ExpectSagaCompleted();
        }
    }
}