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
    IBus bus;

    public CallbackService(IBus bus)
    {
        this.bus = bus;
    }

    public async Task<TResponse> SendRequest(TRequest request)
    {
        if (typeof(TResponse).IsEnum)
        {
            var enumLocal = bus.SendLocal(request);
            return await enumLocal.Register<TResponse>()
                .ConfigureAwait(false);
        }
        if (typeof(TResponse) == typeof(int))
        {
            var intLocal = bus.SendLocal(request);
            object intValue = await intLocal.Register()
                .ConfigureAwait(false);
            return (TResponse)intValue;
        }
        return await bus.SendLocal(request).Register(ar =>
        {
            object[] messages = ar.Messages;
            return (TResponse)messages[0];
        })
        .ConfigureAwait(false);

    }

    public void Dispose()
    {
        ((IDisposable)this).Dispose();
    }
}
#endregion