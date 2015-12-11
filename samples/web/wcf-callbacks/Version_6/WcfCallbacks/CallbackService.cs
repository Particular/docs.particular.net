using System;
using System.ServiceModel;
using System.Threading.Tasks;
using NServiceBus;

#region CallbackService
[ServiceBehavior(
    InstanceContextMode = InstanceContextMode.Single,
    Name = "CallbackService")]
class CallbackService<TRequest, TResponse> : ICallbackService<TRequest, TResponse>
{
    IBusContext busContext;

    public CallbackService(IBusContext busContext)
    {
        this.busContext = busContext;
    }

    public async Task<TResponse> SendRequest(TRequest request)
    {
        SendOptions sendOptions = new SendOptions();
        sendOptions.RouteToLocalEndpointInstance();
        return await busContext.Request<TResponse>(request, sendOptions);
    }

    public void Dispose()
    {
        ((IDisposable)this).Dispose();
    }
}
#endregion