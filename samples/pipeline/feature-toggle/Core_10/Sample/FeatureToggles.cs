using NServiceBus.Features;

#region FeatureToggles
class FeatureToggles : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings.Get<FeatureToggleSettings>();
        var behavior = new FeatureToggleBehavior(settings.Toggles);
        context.Pipeline.Register(behavior, "Optionally skips handlers based on feature toggles");
    }
}
#endregion