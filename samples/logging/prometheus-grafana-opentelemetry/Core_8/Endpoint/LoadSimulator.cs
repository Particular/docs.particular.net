using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

// Simulates busy (almost no delay) / quite time in a sine wave
class LoadSimulator
{
    public LoadSimulator(IEndpointInstance endpointInstance, TimeSpan minimumDelay, TimeSpan idleDuration)
    {
        this.endpointInstance = endpointInstance;
        this.minimumDelay = minimumDelay;
        this.idleDuration = TimeSpan.FromTicks(idleDuration.Ticks / 2);
    }

    public void Start(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => Loop(cancellationToken), cancellationToken);
    }

    async Task Loop(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Work();
                var delay = NextDelay();
                await Task.Delay(delay, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    TimeSpan NextDelay()
    {
        var angleInRadians = Math.PI / 180.0 * ++index;
        var delay = TimeSpan.FromMilliseconds(idleDuration.TotalMilliseconds * Math.Sin(angleInRadians));
        delay += idleDuration;
        delay += minimumDelay;
        return delay;
    }

    Task Work()
    {
        return endpointInstance.SendLocal(new SomeCommand());
    }

    public Task Stop(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    IEndpointInstance endpointInstance;
    TimeSpan minimumDelay;
    TimeSpan idleDuration;
    int index;
}