using NServiceBus.Features;
using NServiceBus.Pipeline;

#region pipeline-config
public class ReplaceStackTraceFeature : Feature
{
    ReplaceStackTraceFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register<Registration>();
    }

    class Registration : RegisterStep
    {
        public Registration()
            : base("ReplaceStackTrace", typeof(ReplaceStackTraceBehavior), "Replace StackTrace")
        {
            InsertAfter("AddExceptionHeaders");
        }
    }
}

#endregion