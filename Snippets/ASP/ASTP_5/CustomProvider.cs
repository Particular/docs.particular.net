﻿using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.AzureTable;

 #region CustomClientProvider

class CustomTableClientProvider : IProvideTableServiceClient
{
    // get fully configured via DI container
    public CustomTableClientProvider(TableServiceClient tableServiceClient)
    {
        Client = tableServiceClient;
    }
    public TableServiceClient Client { get; }
}

// optionally when subscriptions used
class CustomSubscriptionTableClientProvider : IProvideTableServiceClientForSubscriptions
{
    // get fully configured via DI container
    public CustomSubscriptionTableClientProvider(TableServiceClient tableServiceClient)
    {
        Client = tableServiceClient;
    }
    public TableServiceClient Client { get; }
}
#endregion

class CustomClientProviderRegistration
{
    public CustomClientProviderRegistration(EndpointConfiguration endpointConfiguration)
    {
        #region CustomClientProviderRegistration

        endpointConfiguration.RegisterComponents(services => services.AddSingleton<CustomTableClientProvider>());

        // optionally when subscriptions used
        endpointConfiguration.RegisterComponents(services => services.AddSingleton<CustomSubscriptionTableClientProvider>());

        #endregion
    }
}
