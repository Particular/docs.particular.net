namespace Showcase.AcceptanceTests
{
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTesting.Customization;

    // NOTE: There should be one of these for every endpoint that makes up the system
    class OperationsEndpoint : EndpointConfigurationBuilder
    {
        public OperationsEndpoint()
        {
            EndpointSetup<DefaultEndpoint>(config =>
            {
                // NOTE: There is no easy way to specify the configuration of this endpoint that would match what is actually done at runtime
                // NOTE: There is no easy way to ensure the correct handlers and sagas are loaded
                config.TypesToIncludeInScan(typeof(ProvisionDownloadHandler).Assembly.GetTypes());
            });
        }
    }
}