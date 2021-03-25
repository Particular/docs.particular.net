namespace Showcase.AcceptanceTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.AcceptanceTesting.Customization;
    using NServiceBus.AcceptanceTesting.Support;
    using Store.Messages.Commands;

    // NOTE: Had to provide an endpoint setup template. There is not one out of the box
    class DefaultEndpoint : IEndpointSetupTemplate
    {
        public Task<EndpointConfiguration> GetConfiguration(RunDescriptor runDescriptor, EndpointCustomizationConfiguration endpointConfiguration, Action<EndpointConfiguration> configurationBuilderCustomization)
        {
            var configuration = new EndpointConfiguration(endpointConfiguration.EndpointName);

            // NOTE: No way to easily specify where the message types are
            configuration.TypesToIncludeInScan(endpointConfiguration.TypesToInclude
                .Union(typeof(SubmitOrder).Assembly.GetTypes()));

            // NOTE: The transport is not cleaning up in between tests. That means pub-sub infra and queues are not removed.
            configuration.EnableInstallers();

            var recoverability = configuration.Recoverability();
            recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
            recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
            configuration.SendFailedMessagesTo("error");

            // NOTE: This was already in the sample. Customers may not have something like this
            configuration.ApplyCommonConfiguration();

            // NOTE: No way to easily specify a transport. Here it has been hard coded
            //await configuration.DefineTransport(TransportConfiguration, runDescriptor, endpointConfiguration).ConfigureAwait(false);
            //var transport = configuration.UseTransport<LearningTransport>();
                
            configuration.RegisterComponentsAndInheritanceHierarchy(runDescriptor);

            // NOTE: No way to easily specify a persistence. Here it has been hard coded
            //await configuration.DefinePersistence(PersistenceConfiguration, runDescriptor, endpointConfiguration).ConfigureAwait(false);
            //var persistence = configuration.UsePersistence<LearningPersistence>();

            configurationBuilderCustomization(configuration);

            return Task.FromResult(configuration);
        }
    }
}