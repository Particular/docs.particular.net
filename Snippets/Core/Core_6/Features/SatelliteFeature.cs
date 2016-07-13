namespace Core6.Features
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Transports;

   #region SimpleSatelliteFeature

    public class MySatelliteFeature : Feature
    {
        public MySatelliteFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.AddSatelliteReceiver("MyCustomSatellite", "targetQueue", TransportTransactionMode.TransactionScope, PushRuntimeSettings.Default, (builder, messageContext) =>
            {
                // Implement what this satellite needs to do once it receives a message

                var messageId = messageContext.MessageId;
                return Task.FromResult(true);
            });
        }
    }

    #endregion
}
