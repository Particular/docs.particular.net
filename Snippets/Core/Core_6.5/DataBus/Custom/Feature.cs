namespace Core6.DataBus.Custom
{
    using NServiceBus;
    using NServiceBus.Features;

    #region CustomDataBusFeature
    class CustomDatabusFeature : Feature
    {
        public CustomDatabusFeature()
        {
            DependsOn<DataBus>();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var dataBus = new CustomDataBus();
            context.Container.ConfigureComponent(b => dataBus, DependencyLifecycle.SingleInstance);
        }
    }
    #endregion
}
