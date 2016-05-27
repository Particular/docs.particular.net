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
            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
            // Custom code after start

            // Block the process

            // Custom code before stop
            await endpoint.Stop();
            // Custom code after stop
            #endregion
        }

        async Task SendMessagesOutsideMessageHandler()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");
            
            #region 5to6-endpoint-send-messages-outside-handlers
            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
            await endpoint.Send(new SomeMessage());
            #endregion
            await endpoint.Stop();
            // Custom code after stop
            
        }
    }

    class SomeMessage
    {
    }
}
