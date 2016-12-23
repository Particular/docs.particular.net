namespace Core6.Object
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Callbacks.Testing;
    using NServiceBus.Logging;

    class Usage
    {
        async Task Simple(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint, ILog log)
        {
            #region Callbacks-InstanceId

            endpointConfiguration.MakeInstanceUniquelyAddressable("uniqueId");

            #endregion

            #region ObjectCallback

            var message = new Message();
            var response = await endpoint.Request<ResponseMessage>(message)
                .ConfigureAwait(false);
            log.Info($"Callback received with response:{response.Property}");

            #endregion

            #region ObjectCallbackTesting

            var request = new Message();
            var simulatedResponse = new ResponseMessage();

            var session = new TestableCallbackAwareSession();
            session.When((Message msg) => msg == request, simulatedResponse);

            var result = await session.Request<ResponseMessage>(message)
                .ConfigureAwait(false);

            Assert.AreEqual(simulatedResponse, result);

            #endregion

            #region ObjectCallbackTestingWithOptions

            request = new Message();
            simulatedResponse = new ResponseMessage();

            session = new TestableCallbackAwareSession();
            session.When((Message msg, SendOptions o) => msg == request && o.GetHeaders().ContainsKey("Simulated.Header"), simulatedResponse);

            var sendOptions = new SendOptions();
            sendOptions.SetHeader("Simulated.Header", "value");
            result = await session.Request<ResponseMessage>(message, sendOptions)
                .ConfigureAwait(false);

            Assert.AreEqual(simulatedResponse, result);

            #endregion
        }
    }

    static class Assert
    {
        public static void AreEqual(object expected, object result)
        {
        }

        public static void ThrowsAsync<TException>(Func<Task> @delegate)
            where TException : Exception
        {
        }
    }
}