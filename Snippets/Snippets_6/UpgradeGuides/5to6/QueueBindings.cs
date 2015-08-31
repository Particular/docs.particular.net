
namespace Snippets6.UpgradeGuides._5to6
{
    using NServiceBus.Features;
    using NServiceBus.Transports;

    #region 5to6queuebindings
    public class FeatureThatRequiresAQueue : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Settings.Get<QueueBindings>().BindSending("someAddress");
        }
    }
    #endregion
}