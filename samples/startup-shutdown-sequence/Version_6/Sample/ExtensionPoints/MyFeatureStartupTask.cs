using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

public class MyFeatureStartupTask : FeatureStartupTask
{

    protected override Task OnStart(IBusContext context)
    {
        Logger.WriteLine("Inside FeatureStartupTask.OnStart");
        return Task.FromResult(0);
    }
}