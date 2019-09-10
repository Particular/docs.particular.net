namespace Testing_7.Saga
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
        
        #region TestingSagaAsync

        [Test]
        public async Task TestSagaAsync()
        {
            var sagaExpections = await Test.Saga<MySaga>()
                .ExpectReplyToOriginator<MyResponse>()
                .ExpectTimeoutToBeSetIn<StartsSaga>(
                    check: (state, span) =>
                    {
                        return span == TimeSpan.FromDays(7);
                    })
                .ExpectPublish<MyEvent>()
                .ExpectSend<MyCommand>()
                .WhenAsync(
                    sagaIsInvoked: (saga, context) =>
                    {
                        return saga.Handle(new StartsSaga(), context);
                    });

            await sagaExpections
                .ExpectPublish<MyOtherEvent>()
                .ExpectSagaCompleted()
                .WhenSagaTimesOutAsync();
        }

        #endregion
    }
}
