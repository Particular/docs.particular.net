using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class ReceiveCounter
{
    Subject<SomeMessage> messages = new Subject<SomeMessage>();

    public void Subscribe(Action<int> action)
    {
        var observable = from e in messages.Synchronize()
            group e by "" into c
            from v in c.Buffer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            select v.Count;

        observable.Subscribe(action);
    }

    public void OnNext(SomeMessage message)
    {
        messages.OnNext(message);
    }
}