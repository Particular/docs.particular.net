using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Features;

class FeatureDIOld : Feature
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-di-features-old
    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Container.AddSingleton<MySingleton>();
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete

    class MySingleton
    {
    }
}

class FeatureDINew : Feature
{
    #region core-8to9-di-features-new
    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Services.AddSingleton<MySingleton>();
    }
    #endregion

    class MySingleton
    {
    }
}
