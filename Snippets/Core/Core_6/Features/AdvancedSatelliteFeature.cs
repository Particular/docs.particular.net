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
            var satelliteMessagePipeline = context.AddSatellitePipeline("CustomSatellite", TransportTransactionMode.TransactionScope, PushRuntimeSettings.Default, "targetQueue");
            // register the critical error
            satelliteMessagePipeline.Register("Satellite Identifier", b => new MyAdvancedSatelliteBehavior(b.Build<CriticalError>()),
                    "Description of what the advanced satellite does");
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
            criticalError.Raise("Something bad happened - trigger critical error", new Exception("CriticalError occured!!"));
            return Task.FromResult(true);
        }
    }
    #endregion
}
