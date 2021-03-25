namespace Showcase.AcceptanceTests
{
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTesting.Customization;
    using Store.Messages.RequestResponse;

    class ContentManagementEndpoint : EndpointConfigurationBuilder
    {
        public ContentManagementEndpoint()
        {
            EndpointSetup<DefaultEndpoint>(config =>
            {
                config.TypesToIncludeInScan(typeof(OrderAcceptedHandler).Assembly.GetTypes());
                config.ConfigureRouting().RouteToEndpoint(
                    typeof(ProvisionDownloadRequest),
                    typeof(OperationsEndpoint)
                );
            });
        }
    }
}