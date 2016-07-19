namespace Core5.Pipeline.NewStep
{
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region NewPipelineStep

    class NewPipelineStep : RegisterStep
    {
        public NewPipelineStep()
            : base(
                stepId: "NewPipelineStep",
                behavior: typeof(SampleBehavior),
                description: "Logs a warning when processing takes too long")
        {
            // Optional: Specify where it needs to be invoked in the pipeline, for example InsertBefore or InsertAfter
            InsertBefore(WellKnownStep.InvokeHandlers);
        }
    }

    #endregion

    #region AddPipelineStep

    class NewPipelineStepRegistration : INeedInitialization
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            // Register the new step in the pipeline
            var pipeline = busConfiguration.Pipeline;
            pipeline.Register<NewPipelineStep>();
        }
    }

    #endregion

    #region ReplacePipelineStep

    public class ReplaceExistingStep : INeedInitialization
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            var pipeline = busConfiguration.Pipeline;
            pipeline.Replace(
                stepId: "Id of the step to replace",
                newBehavior: typeof(SampleBehavior),
                description: "Description");
        }
    }

    #endregion
}