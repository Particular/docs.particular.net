namespace Core6.Features
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Transports;

    public class MyAdvancedSatelliteFeature : Feature
    {
        public MyAdvancedSatelliteFeature()
        {
            EnableByDefault();
        }

        #region AdvancedSatelliteFeatureSetup
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.AddSatelliteReceiver("CustomSatellite", "targetQueue", TransportTransactionMode.TransactionScope, PushRuntimeSettings.Default, (builder, messageContext) =>
            {
                // To raise a critical error
                var exception = new Exception("CriticalError occurred");
                
                builder.Build<CriticalError>().Raise("Something bad happened - trigger critical error", exception);

                return Task.FromResult(true);
            });
        }
        #endregion
    }
}