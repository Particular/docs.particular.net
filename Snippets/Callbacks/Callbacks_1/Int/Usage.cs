namespace Core6.Int
{
    using System.Threading.Tasks;
    using Core6.Object;
    using NServiceBus;
    using NServiceBus.Callbacks.Testing;
    using NServiceBus.Logging;

    class Usage
    {
        async Task Simple(IEndpointInstance endpoint, ILog log)
        {

            #region IntCallback

            var message = new Message();
            var response = await endpoint.Request<int>(message)
                .ConfigureAwait(false);
            log.Info($"Callback received with response:{response}");

            #endregion

            #region IntCallbackTesting

            var request = new Message();
            var simulatedResponse = 42;

            var session = new TestableCallbackAwareSession();
            session.When((Message msg) => msg == request, simulatedResponse);

            var result = await session.Request<int>(message)
                .ConfigureAwait(false);

            Assert.AreEqual(simulatedResponse, result);

            #endregion
        }
    }
}