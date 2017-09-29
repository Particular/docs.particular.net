using System.Collections.Generic;
using NServiceBus.Features;

#region dispatch-notification-feature
class DispatchNotificationFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var watches = context.Settings.Get<List<IWatchDispatches>>();
        var behavior = new DispatchNotificationBehavior(watches.ToArray());

        context.Pipeline.Register(behavior, "Notifies watches after a dispatch operation");
    }
}
#endregion