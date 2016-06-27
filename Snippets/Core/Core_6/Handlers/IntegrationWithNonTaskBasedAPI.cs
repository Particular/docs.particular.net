namespace Core6
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Threading;
    using System.Threading.Tasks;
    using Encryption.Conventions;
    using NServiceBus;

    #region HandlerWhichIntegratesWithEvent
    public class HandlerWhichIntegratesWithEvent : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var cs = new CancellationTokenSource();
            cs.CancelAfter(TimeSpan.FromSeconds(10));

            var taskCompletionSource = new TaskCompletionSource<object>();

            using (cs.Token.Register(state =>
            {
                var tcs = (TaskCompletionSource<object>) state;
                tcs.TrySetCanceled();
            }, taskCompletionSource))
            {
                var dependency = new DependencyWhichRaisedEvent();
                dependency.MyEvent += (sender, args) => { taskCompletionSource.TrySetResult(null); };

                await taskCompletionSource.Task.ConfigureAwait(false);
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
    public class HandlerWhichIntegratesWithAPM : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var dependency = new DependencyWhichUsesAPM();

            var result = await Task.Factory.FromAsync((callback, state) =>
            {
                var d = (DependencyWhichUsesAPM) state;
                return d.BeginCall(callback, state);
            }, rslt =>
            {
                var d = (DependencyWhichUsesAPM) rslt.AsyncState;
                return d.EndCall(rslt);
            }, dependency)
            .ConfigureAwait(false);
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

    #region HandlerWhichIntegratesWithRemoting

    public class HandlerWhichIntegratesWithRemoting : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var asyncClient = new AsyncClient();

            var result = await asyncClient.Run();
        }
    }

    public class AsyncClient : MarshalByRefObject
    {
        [OneWay]
        public string Callback(IAsyncResult ar)
        {
            var del = (Func<string>)((AsyncResult)ar).AsyncDelegate;
            return del.EndInvoke(ar);
        }

        public Task<string> Run()
        {
            var remoteService = new RemoteService();

            Func<string> remoteCall = remoteService.TimeConsumingRemoteCall;

            return Task.Factory.FromAsync((callback, state) =>
            {
                var call = (Tuple<Func<string>, AsyncClient>) state;
                return call.Item1.BeginInvoke(callback, state);
            }, rslt =>
            {
                var call = (Tuple<Func<string>, AsyncClient>)rslt.AsyncState;
                return call.Item2.Callback(rslt);
            }, Tuple.Create(remoteCall, this));
        }
    }

    #endregion

    public class RemoteService : MarshalByRefObject
    {
        public string TimeConsumingRemoteCall()
        {
            Thread.Sleep(1000);
            return "Hello from remote.";
        }
    }
}