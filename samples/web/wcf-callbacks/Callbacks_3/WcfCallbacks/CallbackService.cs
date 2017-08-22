using System;
using System.ServiceModel;
using System.Threading.Tasks;
using NServiceBus;

#region CallbackService
[ServiceBehavior(
    InstanceContextMode = InstanceContextMode.Single,
    Name = "CallbackService")]
class CallbackService<TRequest, TResponse> :
    ICallbackService<TRequest, TResponse>
{
    IEndpointInstance endpointInstance;

    public CallbackService(IEndpointInstance endpointInstance)
    {
        this.endpointInstance = endpointInstance;
    }

    public Task<TResponse> SendRequest(TRequest request)
    {
        var sendOptions = new SendOptions();
        sendOptions.RouteToThisEndpoint();
        return endpointInstance.Request<TResponse>(request, sendOptions);
    }

    public void Dispose()
    {
        ((IDisposable)this).Dispose();
    }
}
#endregion