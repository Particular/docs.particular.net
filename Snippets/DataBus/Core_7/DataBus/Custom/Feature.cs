namespace Core7.DataBus.Custom
{
    using NServiceBus;
    using NServiceBus.Features;

    #region CustomDataBusFeature
    class CustomDatabusFeature : Feature
    {
        public CustomDatabusFeature()
            => DependsOn<DataBus>();

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Container.ConfigureComponent(typeof(CustomDataBus) , DependencyLifecycle.SingleInstance);
        }
    }
    #endregion
}
