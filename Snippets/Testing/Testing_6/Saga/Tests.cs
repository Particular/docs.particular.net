namespace Testing_6.Saga
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
        public void TestSaga()
        {
            Test.Saga<MySaga>()
                    .ExpectReplyToOriginator<MyResponse>()
                    .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
                    .ExpectPublish<MyEvent>()
                    .ExpectSend<MyCommand>()
                .When((s, context) => s.Handle(new StartsSaga(), context))
                .WhenSagaTimesOut()
                    .ExpectPublish<MyOtherEvent>()
                    .AssertSagaCompletionIs(true);
        }
        #endregion
    }
}