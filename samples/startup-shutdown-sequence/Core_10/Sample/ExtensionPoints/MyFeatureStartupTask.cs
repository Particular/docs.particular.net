using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

public class MyFeatureStartupTask :
    FeatureStartupTask
{
    protected override Task OnStart(IMessageSession session, CancellationToken token)
    {
        Logger.WriteLine("Inside FeatureStartupTask.OnStart");
        return Task.CompletedTask;
    }

    protected override Task OnStop(IMessageSession session, CancellationToken token)
    {
        Logger.WriteLine("Inside FeatureStartupTask.OnStop");
        return Task.CompletedTask;
    }
}