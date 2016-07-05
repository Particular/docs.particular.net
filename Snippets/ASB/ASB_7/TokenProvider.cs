using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;

class TokenProvider
{
    void RegisterTokenProvider(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.NamespaceManagers().TokenProvider(s => Microsoft.ServiceBus.TokenProvider.CreateSharedAccessSignatureTokenProvider("sas"));

        #endregion
    }

    void RegisterTokenProviderUsingNamespaceManagerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider-namespace-manager-settings

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.NamespaceManagers().NamespaceManagerSettingsFactory(s => new NamespaceManagerSettings()
        {
            TokenProvider = Microsoft.ServiceBus.TokenProvider.CreateSharedAccessSignatureTokenProvider("sas")
        });

        #endregion
    }

    void RegisterTokenProviderUsingMessagingFactorySettings(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider-messaging-factory-settings

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.MessagingFactories().MessagingFactorySettingsFactory(s => new MessagingFactorySettings()
        {
            TokenProvider = Microsoft.ServiceBus.TokenProvider.CreateSharedAccessSignatureTokenProvider("sas")
        });

        #endregion
    }
}