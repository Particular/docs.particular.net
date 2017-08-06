using NServiceBus.Features;

namespace Core5.Extending
{
    using NServiceBus;
    using NServiceBus.Configuration.AdvanceExtensibility;

    #region MinimalFeature

    public class MinimalFeature :
        Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    #endregion

    class ComponentBFeature :
        Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    #region DependentFeature

    public class ComponentAFeature :
        Feature
    {
        public ComponentAFeature()
        {
            DependsOn<ComponentBFeature>();
            // Assuming type names are
            // Namespace.ComponentCFeature
            // Namespace.ComponentDFeature
            // Namespace.ComponentEFeature
            DependsOn("ComponentC");
            DependsOnAtLeastOne("ComponentD", "ComponentE");
        }

        #endregion

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    #region FeatureEnabledByDefault

    public class FeatureEnabledByDefault :
        Feature
    {
        public FeatureEnabledByDefault()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    #endregion

    #region FeatureWithDefaults

    public class FeatureWithDefaults :
        Feature
    {
        public FeatureWithDefaults()
        {
            Defaults(s =>
            {
                s.Set("Key", "Value");
                s.SetDefault("OtherKey", 42);
            });
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    #endregion

    public class WriteSettings
    {
        public void ConfigureEndpoint(BusConfiguration busConfiguration)
        {
            #region WriteSettingsFromEndpointConfiguration

            var settings = busConfiguration.GetSettings();
            settings.Set("AnotherKey", new CustomSettingsDto());

            #endregion
        }

        public class CustomSettingsDto
        {
        }
    }

    public class EnablingOtherFeatures :
        Feature
    {
        public EnablingOtherFeatures()
        {
            #region EnablingOtherFeatures

            Defaults(s => s.EnableFeatureByDefault<OtherFeature>());

            #endregion
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
        }

        class OtherFeature :
            Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }

    class FeatureWithPrerequisites :
        Feature
    {
        #region FeatureWithPrerequisites

        public FeatureWithPrerequisites()
        {
            Prerequisite(
                condition: c =>
                {
                    var settings = c.Settings;
                    return settings.HasExplicitValue("SomeKey");
                },
                description: "The key SomeKey was not present.");
        }

        #endregion

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }
}
