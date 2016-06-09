using NServiceBus.Features;

public class MyFeature : Feature
{
    public override void Initialize()
    {
        Logger.WriteLine("Inside Feature.Setup");
    }
}