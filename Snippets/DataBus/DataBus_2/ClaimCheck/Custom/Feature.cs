namespace ClaimCheck_2.ClaimCheck.Custom;

using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ClaimCheck;
using NServiceBus.Features;

#region CustomDataBusFeature

class CustomClaimCheckFeature : Feature
{
    public CustomClaimCheckFeature()
        => DependsOn<ClaimCheck>();

    protected override void Setup(FeatureConfigurationContext context)
        => context.Services.AddSingleton<IClaimCheck, CustomClaimCheck>();
}

#endregion