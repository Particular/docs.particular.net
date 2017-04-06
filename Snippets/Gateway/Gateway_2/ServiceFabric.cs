namespace Gateway_2
{
    using System.Threading.Tasks;
    using System.Fabric;
    using System.Threading;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;

    #region GatewayCommunicationListener
    public class GatewayCommunicationListener : ICommunicationListener
    {
        readonly StatelessServiceContext serviceContext;

        public GatewayCommunicationListener(StatelessServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        public void Abort()
        {
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var endpoint = serviceContext.CodePackageActivationContext.GetEndpoint("RemoteEndpoint");

            string uriPrefix = $"{endpoint.Protocol}://+:{endpoint.Port}/RemoteSite/";

            return uriPrefix.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
        }
    }
    #endregion
}
