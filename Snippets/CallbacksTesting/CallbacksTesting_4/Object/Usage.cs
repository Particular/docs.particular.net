namespace CallbacksTesting2.Object
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Callbacks.Testing;

    class Usage
    {
        async Task ObjectCallbackTesting()
        {
            #region ObjectCallbackTesting

            var request = new Message();
            var simulatedResponse = new ResponseMessage();

            var session = new TestableCallbackAwareSession();
            session.When(
                matcher: (Message message) =>
                {
                    return message == request;
                },
                response: simulatedResponse);

            var result = await session.Request<ResponseMessage>(request)
                .ConfigureAwait(false);

            Assert.AreEqual(simulatedResponse, result);

            #endregion

        }

        async Task ObjectCallbackTestingWithOptions()
        {
            #region ObjectCallbackTestingWithOptions

            var request = new Message();
            var simulatedResponse = new ResponseMessage();

            var session = new TestableCallbackAwareSession();
            session.When(
                matcher: (Message message, SendOptions options) =>
                {
                    return message == request && options.GetHeaders()
                               .ContainsKey("Simulated.Header");
                },
                response: simulatedResponse);

            var sendOptions = new SendOptions();
            sendOptions.SetHeader("Simulated.Header", "value");
            var result = await session.Request<ResponseMessage>(request, sendOptions)
                .ConfigureAwait(false);

            Assert.AreEqual(simulatedResponse, result);

            #endregion
        }
    }

}