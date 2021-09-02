namespace Testing_8.Saga
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    public class AsyncTests
    {
        #region TestingSagaAsync

        [Test]
        public async Task TestSagaAsync()
        {
            var sagaExpectations = await Test.Saga<MySaga>()
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

            await sagaExpectations
                .ExpectPublish<MyOtherEvent>()
                .ExpectSagaCompleted()
                .WhenSagaTimesOutAsync();
        }

        #endregion
    }
}
