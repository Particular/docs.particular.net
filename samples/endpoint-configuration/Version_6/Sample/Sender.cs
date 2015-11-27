using System.Threading.Tasks;
using NServiceBus;

class Sender : IWantToRunWhenBusStartsAndStops
{

    public async Task Start(IBusContext context)
    {
        await context.SendLocal(new MyMessage());
    }

    public Task Stop(IBusContext context)
    {
        return Task.FromResult(0);
    }

}