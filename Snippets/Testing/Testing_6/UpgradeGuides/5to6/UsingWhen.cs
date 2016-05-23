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
                    .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
                    .ExpectPublish<MyEvent>()
                    .ExpectSend<MyCommand>()
            #region 5to6-usingWhen
                .When((s, context) => s.Handle(new StartsSaga(), context))
            #endregion
                .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                    .AssertSagaCompletionIs(true);
        }

        [Test]
        public void NewOverload()
        {
            Test.Saga<MySaga>()
                    .ExpectReplyToOriginator<MyResponse>()
                    .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
                    .ExpectPublish<MyEvent>()
                    .ExpectSend<MyCommand>()
            #region 5to6-usingNewOverload
                .When(s => s.Handle, new StartsSaga())
            #endregion
                .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                    .AssertSagaCompletionIs(true);
        }
    }
}
