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
    IBus bus;

    public CallbackService(IBus bus)
    {
        this.bus = bus;
    }

    public async Task<TResponse> SendRequest(TRequest request)
    {
        SendOptions sendOptions = new SendOptions();
        sendOptions.RouteToLocalEndpointInstance();
        return await bus.Request<TResponse>(request, sendOptions);
    }

    public void Dispose()
    {
        ((IDisposable)this).Dispose();
    }
}
#endregion