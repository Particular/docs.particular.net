using NServiceBus;

#region ReplacePipelineStep
public class ReplaceExistingBehavior : INeedInitialization
{
    public void Customize(BusConfiguration configuration)
    {
        configuration.Pipeline.Replace("Id of the step to replace", typeof(SampleBehavior), "Description");
    }
}
#endregion 
