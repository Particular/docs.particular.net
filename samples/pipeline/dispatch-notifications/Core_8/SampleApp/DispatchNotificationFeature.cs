using System.Collections.Generic;
using NServiceBus.Features;

#region dispatch-notification-feature
class DispatchNotificationFeature :
    Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var watches = context.Settings.Get<List<IDispatchNotifier>>();
        var behavior = new DispatchNotificationBehavior(watches);
        context.Pipeline.Register(behavior, "Notifies dispatch notifiers when a message is dispatched");
    }
}
#endregion