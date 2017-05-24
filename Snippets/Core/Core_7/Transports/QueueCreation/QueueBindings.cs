namespace Core6.Transports
{
    using NServiceBus.Transport;
    using NServiceBus.Features;

    #region queuebindings

    public class FeatureThatRequiresAQueue :
        Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var queueBindings = context.Settings.Get<QueueBindings>();
            queueBindings.BindReceiving("someQueue");
        }
    }

    #endregion
}