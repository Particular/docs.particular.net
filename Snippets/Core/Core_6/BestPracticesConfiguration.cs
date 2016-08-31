namespace Core6
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class BestPracticesConfiguration
    {
        void DisableFeature(EndpointConfiguration endpointConfiguration)
        {
            #region DisableBestPracticeEnforcementPerEndpoint

            var transport = endpointConfiguration.UseTransport<TransportName>();
            var routing = transport.Routing();
            routing.DoNotEnforceBestPractices();

            #endregion
        }

        async Task DisablePerMessage(IPipelineContext context)
        {
            #region DisableBestPracticeEnforcementPerMessage

            var options = new SendOptions();

            options.DoNotEnforceBestPractices();

            await context.Send(new MyEvent(), options)
                .ConfigureAwait(false);

            #endregion
        }

        class MyEvent
        {
        }

        class TransportName : TransportDefinition
        {
            public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new System.NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage { get; }
        }
    }
}