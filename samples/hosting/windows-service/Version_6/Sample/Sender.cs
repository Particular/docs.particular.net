using System.Threading.Tasks;
using NServiceBus;

class Sender : IWantToRunWhenBusStartsAndStops
{

    public async Task Start(IBusSession session)
    {
        await session.SendLocal(new MyMessage());
    }

    public Task Stop(IBusSession session)
    {
        return Task.FromResult(0);
    }

}