using Microsoft.Extensions.DependencyInjection;
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
        context.Services.AddSingleton<CustomLogger>();
    }
}

#endregion