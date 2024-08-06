namespace Core8.DataBus.Custom
{
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus.DataBus;
    using NServiceBus.Features;

    #region CustomDataBusFeature
    class CustomDatabusFeature : Feature
    {
        public CustomDatabusFeature()
            => DependsOn<DataBus>();

        protected override void Setup(FeatureConfigurationContext context)
            => context.Services.AddSingleton<IDataBus, CustomDataBus>();
    }
    #endregion
}
