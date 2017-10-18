namespace Billing
{
    using NServiceBus.Features;

    class NetworkLatencySimulationFeature : Feature
    {
        public NetworkLatencySimulationFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Pipeline.Register(
                builder => new NetworkLatencySimulationBehavior(builder.Build<SimulationEffects>()), 
                "Simulates network latency of 2 seconds during message dispatch"
            );
        }
    }
}