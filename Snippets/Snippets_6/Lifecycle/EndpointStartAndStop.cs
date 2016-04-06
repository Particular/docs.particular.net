﻿namespace Snippets6.Lifecycle
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region lifecycle-EndpointStartAndStop

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
