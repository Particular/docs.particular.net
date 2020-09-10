// ReSharper disable UnusedParameter.Local
namespace Core8.Features
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;

    class MyFeature :
        Feature
    {

#region FeatureSetup
        protected override void Setup(FeatureConfigurationContext context)
        {
            var configurationValue = context.Settings.Get<string>("Key");

            context.Services.AddSingleton(new MessageHandlerDependency());

            context.Pipeline.Register(new CustomBehavior(configurationValue), "custom pipeline behavior");
        }
#endregion

        async Task EndpointConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region EnableDisableFeatures
            // this is not required if the feature uses EnableByDefault()
            endpointConfiguration.EnableFeature<MyFeature>();

            // disable features not in use
            endpointConfiguration.DisableFeature<Sagas>();

            var startableEndpoint = await Endpoint.Create(endpointConfiguration)
                .ConfigureAwait(false);
#endregion
        }

        class MessageHandlerDependency
        {
        }

        class CustomBehavior :
            Behavior<ITransportReceiveContext>
        {
            public CustomBehavior(string configurationValue)
            {
            }

            public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
            {
                throw new NotImplementedException();
            }
        }
    }
}
