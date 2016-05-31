namespace Core6
{
    using System;
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

            var tcs = new TaskCompletionSource<object>();

            using (cs.Token.Register(() => tcs.TrySetCanceled()))
            {
                var dependency = new DependencyWhichRaisedEvent();
                dependency.MyEvent += (sender, args) => { tcs.TrySetResult(null); };

                await tcs.Task.ConfigureAwait(false);
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
            var state = new object();

            var result = await Task.Factory.FromAsync((c, s) => dependency.BeginCall(c, s), dependency.EndCall, state)
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
}