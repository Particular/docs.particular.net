namespace Testing_5.Saga
{
    using System;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    public class Tests
    {
        #region TestingSaga

        [Test]
        public void Run()
        {
            Test.Initialize();
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
                    sagaIsInvoked: saga =>
                    {
                        saga.Handle(new StartsSaga());
                    })
                .WhenSagaTimesOut()
                .ExpectPublish<MyOtherEvent>()
                .AssertSagaCompletionIs(true);
        }

        #endregion
    }
}