using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

/// <summary>
/// Simulates busy (almost no delay) / quite time in a sine wave.
/// </summary>
class LoadSimulator
{
    readonly IEndpointInstance endpointInstance;
    readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
    readonly TimeSpan minimumDelay;
    readonly TimeSpan idleDuration;
    Task fork;

    public LoadSimulator(IEndpointInstance endpointInstance, TimeSpan minimumDelay, TimeSpan idleDuration)
    {
        this.endpointInstance = endpointInstance;
        this.minimumDelay = minimumDelay;
        this.idleDuration = TimeSpan.FromTicks(idleDuration.Ticks / 2);
    }

    public Task Start()
    {
        fork = Loop();
        return Task.CompletedTask;
    }

    async Task Loop()
    {
        try
        {
            while (true)
            {
                await Work()
                    .ConfigureAwait(false);
                var delay = NextDelay();
                await Task.Delay(delay, tokenSource.Token)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    int index;

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

    public Task Stop()
    {
        tokenSource.Cancel();
        return fork;
    }
}