
namespace Snippets6.Extending
{
    using NServiceBus.Features;
    using NServiceBus.Transports;

    #region queuebindings
    public class FeatureThatRequiresAQueue : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Settings.Get<QueueBindings>().BindSending("sendingAddress");
            context.Settings.Get<QueueBindings>().BindReceiving("receivingAddress");
        }
    }
    #endregion
}