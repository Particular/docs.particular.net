namespace Snippets5.Pipeline.Replace
{
    using NServiceBus;

    #region ReplacePipelineStep

    public class ReplaceExistingBehavior : INeedInitialization
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            busConfiguration.Pipeline.Replace("Id of the step to replace", typeof(SampleBehavior), "Description");
        }
    }

    #endregion

}