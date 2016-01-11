using System.Threading.Tasks;
using NServiceBus;

public class Startup : IWantToRunWhenBusStartsAndStops
{
    public async Task Start(IBusSession session)
    {
        await session.SendLocal(new MyMessage());
    }

    public async Task Stop(IBusSession session)
    {
    }
}