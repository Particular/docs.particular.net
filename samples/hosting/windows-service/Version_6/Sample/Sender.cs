using System.Threading.Tasks;
using NServiceBus;

class Sender : IWantToRunWhenBusStartsAndStops
{
    IBus bus;

    public Sender(IBus bus)
    {
        this.bus = bus;
    }

    public async Task StartAsync()
    {
        await bus.SendLocalAsync(new MyMessage());
    }

    public Task StopAsync()
    {
        return Task.FromResult(0);
    }
}