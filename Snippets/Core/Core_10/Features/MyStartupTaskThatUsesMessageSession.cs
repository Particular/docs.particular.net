namespace Core.Features;

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

    protected override async Task OnStart(IMessageSession session, CancellationToken cancellationToken = default)
    {
        await session.Publish(new MyEvent(), cancellationToken);
        resetEvent.Set();
    }

    protected override async Task OnStop(IMessageSession session, CancellationToken cancellationToken = default)
    {
        await session.Publish(new MyEvent(), cancellationToken);
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