using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

// Simulates busy (almost no delay) / quiet time in a sine wave
class LoadSimulator(IMessageSession messageSession, TimeSpan minimumDelay, TimeSpan idleDuration)
{
    CancellationTokenSource tokenSource = new();
    TimeSpan idleDuration = TimeSpan.FromTicks(idleDuration.Ticks / 2);
    Task fork;

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
                await Work();
                var delay = NextDelay();
                await Task.Delay(delay, tokenSource.Token);
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

    Task Work() => messageSession.SendLocal(new SomeCommand());

    public Task Stop()
    {
        tokenSource.Cancel();
        return fork;
    }
}