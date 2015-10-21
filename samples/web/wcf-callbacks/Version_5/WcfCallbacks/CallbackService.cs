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
        if (typeof(TResponse).IsEnum)
        {
            ICallback enumLocal = bus.SendLocal(request);
            return await enumLocal.Register<TResponse>();
        }
        if (typeof(TResponse) == typeof(int))
        {
            ICallback intLocal = bus.SendLocal(request);
            object intValue = await intLocal.Register();
            return (TResponse)intValue;
        }
        return await bus.SendLocal(request).Register(ar =>
        {
            object[] messages = ar.Messages;
            return (TResponse)messages[0];
        });

    }

    public void Dispose()
    {
        ((IDisposable)this).Dispose();
    }
}
#endregion