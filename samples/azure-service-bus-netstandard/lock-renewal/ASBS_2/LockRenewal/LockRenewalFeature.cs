﻿using NServiceBus.Features;

public class LockRenewalFeature : Feature
{
    internal LockRenewalFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register(
            stepId: "LockRenewal",
            factoryMethod: builder => new LockRenewalBehavior(),
            description: "Renew message lock token");
    }
}