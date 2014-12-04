using NServiceBus;
using NServiceBus.Pipeline;

#region NewStepInPipeline
class NewStepInPipeline : RegisterStep
{
    public NewStepInPipeline()
        : base("NewStepInPipeline", typeof(SampleBehavior), "Logs a warning when a message takes too long to process")
    {
        // Optional: Specify where it needs to be invoked in the pipeline, for example InsertBefore or InsertAfter:
        InsertBefore(WellKnownStep.InvokeHandlers);
    }
}

class NewStepInPipelineRegistration : INeedInitialization
{
    public void Customize(BusConfiguration configuration)
    {
        // Register the new step in the pipeline
        configuration.Pipeline.Register<NewStepInPipeline>();
    }
}

#endregion

