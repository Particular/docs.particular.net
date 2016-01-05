using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

public class MyFeatureStartupTask : FeatureStartupTask
{
    protected override Task OnStart(IBusSession session)
    {
        Logger.WriteLine("Inside FeatureStartupTask.OnStart");
        return Task.FromResult(0);
    }

    protected override Task OnStop(IBusSession session)
    {
        Logger.WriteLine("Inside FeatureStartupTask.OnStop");
        return Task.FromResult(0);
    }
}