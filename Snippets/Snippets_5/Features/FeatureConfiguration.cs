namespace Snippets5.Features
{
    using System;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;

    class MyMagicFeature : Feature
    {

#region FeatureConfiguration
        public MyMagicFeature()
        {
            EnableByDefault();

            DependsOn<Sagas>();

            Prerequisite(c => c.Settings.EndpointName().StartsWith("Magical"), "This feature only works on magical endpoints");
        }
#endregion

#region FeatureSetup
        protected override void Setup(FeatureConfigurationContext context)
        {
            // retrieve some global settings
            var supervised = context.Settings.Get<bool>("SupervisorActive");

            if (!supervised)
            {
                // register a more performant cleanup process with the container
                context.Container.ConfigureComponent<UnsafeCleanupComponent>(DependencyLifecycle.InstancePerCall);
            }

            // add a custom behavior to the message processing pipeline which does our cleanup.
            context.Pipeline.Register<CleanupBehaviorRegistration>();
        }
#endregion

        void ConfigureFeatures()
        {
#region EnableDisableFeatures
            var configuration = new BusConfiguration();
            
            // enable Saga feature since our custom feature relies on it
            configuration.EnableFeature<Sagas>();

            // this is not required if the feature uses EnableByDefault()
            configuration.EnableFeature<MyMagicFeature>();

            // it's best practice for magical endpoints to disable Second Level Retries
            configuration.DisableFeature<SecondLevelRetries>();

            var bus = Bus.Create(configuration);
        }
#endregion
    }

    class UnsafeCleanupComponent
    {
    }

    class CleanupBehaviorRegistration : RegisterStep
    {
        public CleanupBehaviorRegistration(string stepId, Type behavior, string description) : base(stepId, behavior, description)
        {
        }

        public CleanupBehaviorRegistration() : base(string.Empty, typeof(object), "does some custom cleanup tasks")
        {
        }
    }
}
