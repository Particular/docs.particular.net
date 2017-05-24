namespace Core7.Features
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    #region MyStartupTaskThatUsesMessageSession

    class MyStartupTaskThatUsesMessageSession :
        FeatureStartupTask,
        IDisposable
    {
        ManualResetEventSlim resetEvent = new ManualResetEventSlim();

        protected override async Task OnStart(IMessageSession session)
        {
            await session.Publish<MyEvent>()
                .ConfigureAwait(false);
            resetEvent.Set();
        }

        protected override async Task OnStop(IMessageSession session)
        {
            await session.Publish<MyEvent>()
                .ConfigureAwait(false);
            resetEvent.Reset();
        }

        public void Dispose()
        {
            resetEvent.Dispose();
        }
    }

    #endregion

    class MyEvent
    {
    }
}