using NServiceBus.Features;

namespace Core5.Extending
{
    #region MinimalFeature
    public class MinimalFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {

        }
    }
    #endregion

    class DependencyAFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }

    #region DependentFeature
    public class DependentFeature : Feature
    {
        public DependentFeature()
        {
            DependsOn<DependencyAFeature>();
            DependsOn("DependencyB");
            DependsOnAtLeastOne("DependencyC", "DependencyD");
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }
    #endregion

    #region FeatureEnabledByDefault
    public class FeatureEnabledByDefault : Feature
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
    public class FeatureWithDefaults : Feature
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

    public class EnablingOtherFeatures : Feature
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

        class OtherFeature : Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }

    class FeatureWithPrerequisites : Feature
    {
        #region FeatureWithPrerequisites
        public FeatureWithPrerequisites()
        {
            Prerequisite(c => c.Settings.HasExplicitValue("SomeKey"),"The key SomeKey was not present.");
        }
        #endregion

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }
}