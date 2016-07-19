namespace Core6.Pipeline
{
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region NewPipelineStep

    class NewPipelineStep :
        RegisterStep
    {
        public NewPipelineStep()
            : base(
                stepId: "NewPipelineStep",
                behavior: typeof(SampleBehavior),
                description: "Logs a warning when processing takes too long")
        {
        }
    }

    #endregion

    #region AddPipelineStep

    class NewPipelineStepRegistration :
        INeedInitialization
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // Register the new step in the pipeline
            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register<NewPipelineStep>();
        }
    }

    #endregion

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