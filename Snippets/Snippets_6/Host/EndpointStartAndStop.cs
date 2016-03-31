namespace Snippets6.Host
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region host-EndpointStartAndStop

    // When using NServiceBus.Host or NService.Host.AzureCloudService
    class RunWhenEndpointStartsAndStops : IWantToRunWhenEndpointStartsAndStops
    {
        public async Task Start(IMessageSession session)
        {
            // perform startup logic
        }

        public async Task Stop(IMessageSession session)
        {
            // perform shutdown logic
        }
    }

    #endregion
}
