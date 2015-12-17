using System.Threading.Tasks;
using NServiceBus;

class Sender : IWantToRunWhenBusStartsAndStops
{

    public async Task Start(IBusContext busContext)
    {
        await busContext.SendLocal(new MyMessage());
    }

    public Task Stop(IBusContext busContext)
    {
        return Task.FromResult(0);
    }

}