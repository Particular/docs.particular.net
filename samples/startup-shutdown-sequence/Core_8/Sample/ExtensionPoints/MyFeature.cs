using NServiceBus.Features;

public class MyFeature :
    Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        Logger.WriteLine("Inside Feature.Setup");
        context.RegisterStartupTask(new MyFeatureStartupTask());
    }
}