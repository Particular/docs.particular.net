namespace CallbacksTesting1.Enum
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Callbacks.Testing;

    class Usage
    {
        async Task Simple()
        {
            #region EnumCallbackTesting

            var request = new Message();
            var simulatedResponse = Status.OK;

            var session = new TestableCallbackAwareSession();
            session.When(
                matcher: (Message message) =>
                {
                    return message == request;
                },
                response: simulatedResponse);

            var result = await session.Request<Status>(request)
                .ConfigureAwait(false);

            Assert.AreEqual(simulatedResponse, result);

            #endregion
        }
    }
}