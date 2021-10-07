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
            settings.SetDefault<LockRenewalOptions>(new LockRenewalOptions
            {
                // NServiceBus.Transport.AzureServiceBus sets LockDuration to 5 minutes by default
                LockDuration = TimeSpan.FromMinutes(5),
                ExecuteRenewalBefore = TimeSpan.FromSeconds(10)
            });
        });

        #endregion
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var lockRenewalOptions = context.Settings.Get<LockRenewalOptions>();
        var renewLockTokenIn = lockRenewalOptions.LockDuration - lockRenewalOptions.ExecuteRenewalBefore;

        context.Pipeline.Register(
            stepId: "LockRenewal",
            factoryMethod: builder => new LockRenewalBehavior(renewLockTokenIn),
            description: "Renew message lock token");
    }
}