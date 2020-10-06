namespace Core8.Pipeline
{
    using NServiceBus;
    using NServiceBus.Features;

    class BehaviorRegistration
    {
        void Configure(EndpointConfiguration endpointConfiguration)
        {
            #region RegisterBehaviorEndpointConfiguration

            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register(
                behavior: new SampleBehavior(),
                description: "Logs a warning when processing takes too long");

            #endregion
        }
    }

    class FeatureWithBehavior :
        Feature
    {
        #region RegisterBehaviorFromFeature

        protected override void Setup(FeatureConfigurationContext context)
        {
            var pipeline = context.Pipeline;
            pipeline.Register(
                behavior: new SampleBehavior(),
                description: "Logs a warning when processing takes too long");
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

    #region RegisterOrReplaceStep

    public class RegisterOrReplaceStep :
        INeedInitialization
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var pipeline = endpointConfiguration.Pipeline;
            pipeline.RegisterOrReplace(
                stepId: "StepIdThatMayOrMayNotExist",
                behavior: typeof(SampleBehavior),
                description: "Description");
        }
    }

    #endregion
}