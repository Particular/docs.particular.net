using NServiceBus.Features;

public class MyFeatureStartupTask:FeatureStartupTask
{
    protected override void OnStart()
    {
        Logger.WriteLine("Inside FeatureStartupTask.OnStart");
    }
}