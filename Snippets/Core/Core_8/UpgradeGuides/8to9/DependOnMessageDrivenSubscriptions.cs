using NServiceBus.Features;

class DependOnMessageDrivenSubscriptions
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-depend-on-subscriptions
    class MyFeature : Feature
    {
        public MyFeature() => DependsOn<MessageDrivenSubscriptions>();

        protected override void Setup(FeatureConfigurationContext context)
        {
            // setup my feature
        }
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
}