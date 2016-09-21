namespace Testing_6.Saga
{
    using System;
    using System.Threading.Tasks;
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
                    .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                    .AssertSagaCompletionIs(true);
        }

        //[Test]
        //public async Task TestSaga_AAA()
        //{
        //    var saga = new MySaga();
        //    var context = new TestableMessageHandlerContext();
        //    var message = new StartsSaga();

        //    //TODO: get it working with replytooriginator and requesttimoeut in side the saga handler :/
        //    //await saga.Handle(message, context)
        //    //    .ConfigureAwait(false);

        //    //var processMessage = context.SentMessages[0].Message;
        //}
        #endregion
    }
}