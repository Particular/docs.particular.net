namespace Core8
{
    using System.Threading.Tasks;
    using NServiceBus;

    class SendOnly
    {
        async Task Configure()
        {
            #region send-only-endpoint

            var endpointConfiguration = new EndpointConfiguration("EndpointName");
            endpointConfiguration.SendOnly();
            // Apply other necessary endpoint configuration, e.g. transport
            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            #endregion
        }
    }
}
