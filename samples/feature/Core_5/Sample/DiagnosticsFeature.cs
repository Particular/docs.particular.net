using NServiceBus;
using NServiceBus.Features;

#region DiagnosticsFeature

public class DiagnosticsFeature :
    Feature
{
    internal DiagnosticsFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var container = context.Container;
        container.ConfigureComponent<CustomLogger>(DependencyLifecycle.SingleInstance);
    }
}

#endregion