namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class EndpointStartStop
    {
        async Task StartEndpoint()
        {
            #region 5to6-endpoint-start-stop
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");

            // Custom code before start
            IEndpointInstance endpointInstance = await Endpoint.Start(endpointConfiguration);
            // Custom code after start

            // Block the process

            // Custom code before stop
            await endpointInstance.Stop();
            // Custom code after stop
            #endregion
        }

        async Task SendMessagesOutsideMessageHandler(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-endpoint-send-messages-outside-handlers
            IEndpointInstance endpointInstance = await Endpoint.Start(endpointConfiguration);
            var messageSession = (IMessageSession)endpointInstance;
            await messageSession.Send(new SomeMessage());
            #endregion
        }
    }

    class SomeMessage
    {
    }
}
