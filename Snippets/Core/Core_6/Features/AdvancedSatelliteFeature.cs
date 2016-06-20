namespace Core6.Features
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;
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
            var satellitePipeline = context.AddSatellitePipeline("CustomSatellite", TransportTransactionMode.TransactionScope, PushRuntimeSettings.Default, "targetQueue");
            // register the critical error
            satellitePipeline.Register(
                stepId: "Satellite Identifier",
                factoryMethod: builder => new MyAdvancedSatelliteBehavior(builder.Build<CriticalError>()),
                description: "Description of what satellite does");
        }

        #endregion
    }

    #region AdvancedSatelliteBehavior
    class MyAdvancedSatelliteBehavior : PipelineTerminator<ISatelliteProcessingContext>
    {
        CriticalError criticalError;

        public MyAdvancedSatelliteBehavior(CriticalError criticalError)
        {
            this.criticalError = criticalError;
        }

        protected override Task Terminate(ISatelliteProcessingContext context)
        {
            // To raise a critical error
            var exception = new Exception("CriticalError occurred");
            criticalError.Raise("Something bad happened - trigger critical error", exception);
            return Task.FromResult(true);
        }
    }
    #endregion
}
