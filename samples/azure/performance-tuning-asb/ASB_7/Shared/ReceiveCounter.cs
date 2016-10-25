using System;
using System.Threading;

public class ReceiveCounter
{
    static int count;
    Timer timer;
    Action<int> action;

    public void Subscribe(Action<int> action)
    {
        this.action = action;
        timer = new Timer(Callback, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
    }

    void Callback(object state)
    {
        var value = Interlocked.Exchange(ref count, 0);
        action(value);
    }

    public void IncreaseNumberOfReceivedMessages()
    {
        Interlocked.Increment(ref count);
    }
}