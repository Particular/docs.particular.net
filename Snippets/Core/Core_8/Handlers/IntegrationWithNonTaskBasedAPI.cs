namespace Core8.Handlers
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Remoting.Messaging;
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
                    var completionSource = (TaskCompletionSource<object>) state;
                    completionSource.TrySetCanceled();
                },
                state: taskCompletionSource))
            {
                var dependency = new DependencyWhichRaisedEvent();
                dependency.MyEvent += (sender, args) =>
                {
                    taskCompletionSource.TrySetResult(null);
                };

                await taskCompletionSource.Task
                    .ConfigureAwait(false);
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

    #region HandlerWhichIntegratesWithAPM

    public class HandlerWhichIntegratesWithAPM :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var dependency = new DependencyWhichUsesAPM();

            var result = await Task.Factory.FromAsync(
                    beginMethod: (callback, state) =>
                    {
                        var d = (DependencyWhichUsesAPM) state;
                        return d.BeginCall(callback, state);
                    },
                    endMethod: asyncResult =>
                    {
                        var d = (DependencyWhichUsesAPM) asyncResult.AsyncState;
                        return d.EndCall(asyncResult);
                    },
                    state: dependency)
                .ConfigureAwait(false);

            // Use the result in some way
            Trace.WriteLine(result);
        }
    }

    #endregion

    public class DependencyWhichUsesAPM
    {
        public IAsyncResult BeginCall(AsyncCallback callback, object state)
        {
            return null;
        }

        public string EndCall(IAsyncResult callback)
        {
            return null;
        }
    }

    #region HandlerWhichIntegratesWithRemotingWithAPM

    public class HandlerWhichIntegratesWithRemotingWithAPM :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var asyncClient = new AsyncClient();
            var result = await asyncClient.Run()
                .ConfigureAwait(false);
            // Use the result in some way
            Trace.WriteLine(result);
        }
    }

    public class AsyncClient :
        MarshalByRefObject
    {
        [OneWay]
        public string Callback(IAsyncResult ar)
        {
            var asyncDelegate = (Func<string>) ((AsyncResult) ar).AsyncDelegate;
            return asyncDelegate.EndInvoke(ar);
        }

        public Task<string> Run()
        {
            var remoteService = new RemoteService();

            Func<string> remoteCall = remoteService.TimeConsumingRemoteCall;

            return Task.Factory.FromAsync(
                beginMethod: (callback, state) =>
                {
                    var call = (Tuple<Func<string>, AsyncClient>) state;
                    return call.Item1.BeginInvoke(callback, state);
                },
                endMethod: asyncResult =>
                {
                    var call = (Tuple<Func<string>, AsyncClient>) asyncResult.AsyncState;
                    return call.Item2.Callback(asyncResult);
                },
                state: Tuple.Create(remoteCall, this));
        }
    }

    #endregion

    #region HandlerWhichIntegratesWithRemotingWithTask

    public class HandlerWhichIntegratesWithRemotingWithTask :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var result = await Task.Run(
                function: () =>
                {
                    var remoteService = new RemoteService();
                    return remoteService.TimeConsumingRemoteCall();
                })
                .ConfigureAwait(false);
            // Use the result in some way
            Trace.WriteLine(result);
        }
    }

    #endregion

    public class RemoteService :
        MarshalByRefObject
    {
        public string TimeConsumingRemoteCall()
        {
            Thread.Sleep(1000);
            return "Hello from remote.";
        }
    }

}