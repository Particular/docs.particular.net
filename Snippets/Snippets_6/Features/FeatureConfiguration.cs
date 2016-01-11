// ReSharper disable UnusedParameter.Local
namespace Snippets6.Features
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;

    class SecondLevelRetries : Feature
    {

#region FeatureSetup
        protected override void Setup(FeatureConfigurationContext context)
        {
            SecondLevelRetriesConfig retriesConfig = context.Settings.GetConfigSection<SecondLevelRetriesConfig>();

            SecondLevelRetryPolicy retryPolicy = new SecondLevelRetryPolicy(retriesConfig.NumberOfRetries, retriesConfig.TimeIncrease);
            context.Container.RegisterSingleton(typeof(SecondLevelRetryPolicy), retryPolicy);

            context.Pipeline.Register<SecondLevelRetriesBehavior.Registration>();
        }
#endregion

        async Task EndpointConfiguration()
        {
#region EnableDisableFeatures
            BusConfiguration configuration = new BusConfiguration();
            
            // enable delayed delivery feature since SLR relies on it
            configuration.EnableFeature<DelayedDeliveryFeature>();

            // this is not required if the feature uses EnableByDefault()
            configuration.EnableFeature<SecondLevelRetries>();

            // we can disable features we won't use
            configuration.DisableFeature<Sagas>();

            IStartableEndpoint endpoint = await Endpoint.Create(configuration);
#endregion
        }

        class SecondLevelRetriesBehavior
        {
            public class Registration : RegisterStep
            {
                public Registration()
                    : base(string.Empty, typeof(object), null)
                {
                }
            }
        }

        class SecondLevelRetryPolicy
        {
            public SecondLevelRetryPolicy(int numberOfRetries, TimeSpan timeIncrease)
            {
            }
        }

        class DelayedDeliveryFeature : Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
