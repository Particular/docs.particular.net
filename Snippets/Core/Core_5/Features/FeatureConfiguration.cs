// ReSharper disable UnusedParameter.Local
namespace Core5.Features
{
    using System;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;

    class SecondLevelRetries : Feature
    {

#region FeatureSetup
        protected override void Setup(FeatureConfigurationContext context)
        {
            var retriesConfig = context.Settings.GetConfigSection<SecondLevelRetriesConfig>();

            var retryPolicy = new SecondLevelRetryPolicy(retriesConfig.NumberOfRetries, retriesConfig.TimeIncrease);
            context.Container.RegisterSingleton(typeof(SecondLevelRetryPolicy), retryPolicy);

            context.Pipeline.Register<SecondLevelRetriesBehavior.Registration>();
        }
#endregion

        void EndpointConfiguration(BusConfiguration busConfiguration)
        {
#region EnableDisableFeatures
            // enable delayed delivery feature since SLR relies on it
            busConfiguration.EnableFeature<DelayedDeliveryFeature>();

            // this is not required if the feature uses EnableByDefault()
            busConfiguration.EnableFeature<SecondLevelRetries>();

            // disable features not in use
            busConfiguration.DisableFeature<Sagas>();

            var startableBus = Bus.Create(busConfiguration);
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
