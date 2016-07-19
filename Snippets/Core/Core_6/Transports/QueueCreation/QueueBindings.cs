namespace Core6.Transports.QueueCreation
{
    using NServiceBus.Features;
    using NServiceBus.Transport;

    #region queuebindings
    public class FeatureThatRequiresAQueue :
        Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Settings.Get<QueueBindings>().BindSending("sendingAddress");
            context.Settings.Get<QueueBindings>().BindReceiving("receivingAddress");
        }
    }
    #endregion
}