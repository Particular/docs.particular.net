// ReSharper disable UnusedParameter.Local
namespace Core6.Features
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;

    class SecondLevelRetries :
        Feature
    {

#region FeatureSetup
        protected override void Setup(FeatureConfigurationContext context)
        {
            var settings = context.Settings;
            var retriesConfig = settings.GetConfigSection<SecondLevelRetriesConfig>();

            var retryPolicy = new SecondLevelRetryPolicy(
                numberOfRetries: retriesConfig.NumberOfRetries,
                timeIncrease: retriesConfig.TimeIncrease);
            var container = context.Container;
            container.RegisterSingleton(typeof(SecondLevelRetryPolicy), retryPolicy);

            var pipeline = context.Pipeline;
            pipeline.Register<SecondLevelRetriesBehavior.Registration>();
        }
#endregion

        async Task EndpointConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region EnableDisableFeatures
            // enable delayed delivery feature since SLR relies on it
            endpointConfiguration.EnableFeature<DelayedDeliveryFeature>();

            // this is not required if the feature uses EnableByDefault()
            endpointConfiguration.EnableFeature<SecondLevelRetries>();

            // disable features not in use
            endpointConfiguration.DisableFeature<Sagas>();

            var startableEndpoint = await Endpoint.Create(endpointConfiguration)
                .ConfigureAwait(false);
#endregion
        }

        class SecondLevelRetriesBehavior
        {
            public class Registration :
                RegisterStep
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

        class DelayedDeliveryFeature :
            Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
