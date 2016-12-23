namespace Core6.Enum
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
            #region EnumCallback

            var message = new Message();
            var response = await endpoint.Request<Status>(message)
                .ConfigureAwait(false);
            log.Info($"Callback received with response:{response}");

            #endregion

            #region EnumCallbackTesting

            var request = new Message();
            var simulatedResponse = Status.OK;

            var session = new TestableCallbackAwareSession();
            session.When((Message msg) => msg == request, simulatedResponse);

            var result = await session.Request<Status>(message)
                .ConfigureAwait(false);

            Assert.AreEqual(simulatedResponse, result);

            #endregion
        }
    }
}