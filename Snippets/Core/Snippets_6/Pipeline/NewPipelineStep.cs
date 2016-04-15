namespace Snippets6.Pipeline
{
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region NewPipelineStep

    class NewPipelineStep : RegisterStep
    {
        public NewPipelineStep()
            : base("NewPipelineStep", typeof(SampleBehavior), "Logs a warning when processing takes too long")
        {
            // Optional: Specify where it needs to be invoked in the pipeline, for example InsertBefore or InsertAfter
            InsertBefore(WellKnownStep.InvokeHandlers);
        }
    }

    #endregion

    #region AddPipelineStep

    class NewPipelineStepRegistration : INeedInitialization
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // Register the new step in the pipeline
            endpointConfiguration.Pipeline.Register<NewPipelineStep>();
        }
    }

    #endregion

    #region ReplacePipelineStep

    public class ReplaceExistingStep : INeedInitialization
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.Pipeline.Replace("Id of the step to replace", typeof(SampleBehavior), "Description");
        }
    }

    #endregion
}