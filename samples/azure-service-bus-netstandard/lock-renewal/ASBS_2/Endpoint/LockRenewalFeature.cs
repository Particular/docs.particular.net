using System;
using NServiceBus.Features;

public class LockRenewalFeature : Feature
{
    internal LockRenewalFeature()
    {
        #region LockRenewalFeature

        EnableByDefault();

        Defaults(settings =>
        {
            // NServiceBus.Transport.AzureServiceBus sets LockDuration to 5 minutes by default
            settings.SetDefault("LockDurationAsTimeSpan", TimeSpan.FromMinutes(5));
            settings.SetDefault("RenewBeforeAsTimeSpan", TimeSpan.FromSeconds(10));
        });

        #endregion
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var maxLockDuration = context.Settings.Get<TimeSpan>("LockDurationAsTimeSpan");
        var renewBefore = context.Settings.Get<TimeSpan>("RenewBeforeAsTimeSpan");
        var renewLockTokenIn = maxLockDuration - renewBefore;

        context.Pipeline.Register(
            stepId: "LockRenewal",
            factoryMethod: builder => new LockRenewalBehavior(renewLockTokenIn),
            description: "Renew message lock token");
    }
}