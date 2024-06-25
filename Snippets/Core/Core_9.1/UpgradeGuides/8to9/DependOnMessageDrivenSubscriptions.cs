using NServiceBus.Features;

class DependOnMessageDrivenSubscriptions
{
    #region core-8to9-depend-on-subscriptions
    class MyFeature : Feature
    {
        public MyFeature() => DependsOn("NServiceBus.Features.MessageDrivenSubscriptions");

        protected override void Setup(FeatureConfigurationContext context)
        {
            // setup my feature
        }
    }
    #endregion
}