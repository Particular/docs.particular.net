namespace Core6.Pipeline
{
    using NServiceBus;
    using NServiceBus.Features;

    class BehaviorRegistration
    {
        void Configure(EndpointConfiguration endpointConfiguration)
        {
            #region RegisterBehaviorEndpointConfiguration

            endpointConfiguration.Pipeline.Register(new SampleBehavior(), "Logs a warning when processing takes too long");

            #endregion
        }
    }

    class FeatureWithBehavior : Feature
    {
        #region RegisterBehaviorFromFeature

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Pipeline.Register(new SampleBehavior(), "Logs a warning when processing takes too long");
        }

        #endregion
    }

    #region ReplacePipelineStep

    public class ReplaceExistingStep :
        INeedInitialization
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Replace(
                stepId: "Id of the step to replace",
                newBehavior: typeof(SampleBehavior),
                description: "Description");
        }
    }

    #endregion
}