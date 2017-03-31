namespace Core6.ServiceFabricHosting
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using NServiceBus;

    #region EndpointCommunicationListener

    public class EndpointCommunicationListener : ICommunicationListener
    {
        IEndpointInstance endpointInstance;

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var endpointName = "Sales";

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            // endpoint configuration

            endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            return endpointName;
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            return endpointInstance.Stop();
        }

        public void Abort()
        {
            CloseAsync(CancellationToken.None);
        }
    }

    #endregion
}