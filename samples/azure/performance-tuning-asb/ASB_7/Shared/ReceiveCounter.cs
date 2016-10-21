using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

public class ReceiveCounter
{
    static int count;
    Timer timer;
    Action<int> action;

    public void Subscribe(Action<int> action)
    {
        this.action = action;
        timer = new Timer
        {
            Interval = 1000,
            AutoReset = true
        };
        timer.Elapsed += TimerOnElapsed;
        timer.Start();
    }

    void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
    {
        var value = Interlocked.Exchange(ref count, 0);
        action(value);
    }

    public void IncreaseNumberOfReceivedMessages()
    {
        Interlocked.Increment(ref count);
    }
}