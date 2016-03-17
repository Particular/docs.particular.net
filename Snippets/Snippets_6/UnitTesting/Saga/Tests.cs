namespace Snippets6.UnitTesting.Saga
{
    using System;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    #region TestingSaga

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Run()
        {
            Test.Saga<MySaga>()
                    .ExpectReplyToOriginator<MyResponse>()
                    .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
                    .ExpectPublish<MyEvent>()
                    .ExpectSend<MyCommand>()
                .When((s, context) => s.Handle(new StartsSaga(), context))
                    .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                    .AssertSagaCompletionIs(true);
        }
    }

    #endregion
}