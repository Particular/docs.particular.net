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

    public Task<string> OpenAsync(CancellationToken cancellationToken)
    {
        var endpoint = serviceContext.CodePackageActivationContext.GetEndpoint("RemoteEndpoint");

        var uriPrefix = $"{endpoint.Protocol}://+:{endpoint.Port}/RemoteSite/";

        var result = uriPrefix.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
        return Task.FromResult(result);
    }
}
#endregion
