namespace Core.Handlers;

using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

#region HandlerWhichIntegratesWithEvent

public class HandlerWhichIntegratesWithEvent :
    IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var cancellationToken = new CancellationTokenSource();
        cancellationToken.CancelAfter(TimeSpan.FromSeconds(10));

        var taskCompletionSource = new TaskCompletionSource<object>();

        using (cancellationToken.Token.Register(
                   callback: state =>
                   {
                       var completionSource = (TaskCompletionSource<object>)state;
                       completionSource.TrySetCanceled();
                   },
                   state: taskCompletionSource))
        {
            var dependency = new DependencyWhichRaisedEvent();
            dependency.MyEvent += (sender, args) =>
            {
                taskCompletionSource.TrySetResult(null);
            };

            await taskCompletionSource.Task;
        }
    }
}

#endregion

public class DependencyWhichRaisedEvent
{
    public event EventHandler MyEvent;

    protected virtual void OnMyEvent()
    {
        MyEvent?.Invoke(this, EventArgs.Empty);
    }
}