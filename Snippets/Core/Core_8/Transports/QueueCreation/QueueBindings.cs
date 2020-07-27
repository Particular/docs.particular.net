namespace Core7.Transports.QueueCreation
{
    using NServiceBus.Features;
    using NServiceBus.Transport;

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