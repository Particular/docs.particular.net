namespace Core6.UpgradeGuides._5to6
{
    using NServiceBus.Features;

    #region 5to6-DependentFeature

    public class DependentFeature : Feature
    {
        public DependentFeature()
        {
            DependsOn("Namespace.DependencyB");
            DependsOnAtLeastOne("Namespace.DependencyC", "Namespace.DependencyD");
        }

        #endregion

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }
}