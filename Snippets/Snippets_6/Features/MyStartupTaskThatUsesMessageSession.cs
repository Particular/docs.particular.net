namespace Snippets6.Features
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    #region MyStartupTaskThatUsesMessageSession

    class MyStartupTaskThatUsesMessageSession : FeatureStartupTask, IDisposable
    {
        ManualResetEventSlim resetEvent = new ManualResetEventSlim();

        protected override async Task OnStart(IMessageSession session)
        {
            await session.Publish<MyEvent>();
            resetEvent.Set();
        }

        protected override async Task OnStop(IMessageSession session)
        {
            await session.Publish<MyEvent>();
            resetEvent.Reset();
        }

        public void Dispose()
        {
            resetEvent.Dispose();
        }
    }

    #endregion

    internal class MyEvent
    {
    }
}