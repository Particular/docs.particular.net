namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus.Features;

    #region 5to6-DependentFeature

    public class DependentFeature : Feature
    {
        public DependentFeature()
        {
            DependsOn("DependencyB");
            DependsOnAtLeastOne("DependencyC", "DependencyD");
        }

        #endregion

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }
}