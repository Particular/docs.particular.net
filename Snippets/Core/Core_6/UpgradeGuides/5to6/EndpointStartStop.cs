namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class EndpointStartStop
    {
        async Task StartEndpoint()
        {
            #region 5to6-endpoint-start-stop
            var endpointConfiguration = new EndpointConfiguration("EndpointName");

            // Custom code before start
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            // Custom code after start

            // Block the process

            // Custom code before stop
            await endpointInstance.Stop()
                .ConfigureAwait(false);
            // Custom code after stop
            #endregion
        }

        async Task SendMessagesOutsideMessageHandler(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-endpoint-send-messages-outside-handlers
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            var messageSession = (IMessageSession)endpointInstance;
            await messageSession.Send(new SomeMessage())
                .ConfigureAwait(false);
            #endregion
        }
    }

    class SomeMessage
    {
    }
}
