using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Features;

class FeatureDI : Feature
{
    #region core-8to9-di-features
    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Services.AddSingleton<MySingleton>();
    }
    #endregion

    class MySingleton
    {
    }
}