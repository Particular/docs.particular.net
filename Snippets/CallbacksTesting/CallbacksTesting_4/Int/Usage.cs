namespace CallbacksTesting2.Int
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Callbacks.Testing;

    class Usage
    {
        async Task Simple()
        {
            #region IntCallbackTesting

            var request = new Message();
            var simulatedResponse = 42;

            var session = new TestableCallbackAwareSession();
            session.When(
                matcher: (Message message) =>
                {
                    return message == request;
                },
                response: simulatedResponse);

            var result = await session.Request<int>(request)
                .ConfigureAwait(false);

            Assert.AreEqual(simulatedResponse, result);

            #endregion
        }
    }
}