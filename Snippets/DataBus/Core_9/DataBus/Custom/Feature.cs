namespace Core9.DataBus.Custom
{
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus.DataBus;
    using NServiceBus.Features;

#pragma warning disable CS0618 // Type or member is obsolete
    #region CustomDataBusFeature
    class CustomDatabusFeature : Feature
    {
        public CustomDatabusFeature()
            => DependsOn<DataBus>();

        protected override void Setup(FeatureConfigurationContext context)
            => context.Services.AddSingleton<IDataBus, CustomDataBus>();
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
}
