using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

public class MyFeatureStartupTask :
    FeatureStartupTask
{
    protected override Task OnStart(IMessageSession session)
    {
        Logger.WriteLine("Inside FeatureStartupTask.OnStart");
        return Task.CompletedTask;
    }

    protected override Task OnStop(IMessageSession session)
    {
        Logger.WriteLine("Inside FeatureStartupTask.OnStop");
        return Task.CompletedTask;
    }
}