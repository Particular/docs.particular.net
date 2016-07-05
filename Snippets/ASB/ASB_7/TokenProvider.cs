using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;

class TokenProvider
{
    void RegisterTokenProvider(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespaceManagers = transport.NamespaceManagers();
        namespaceManagers.TokenProvider(s =>
        {
            return Microsoft.ServiceBus.TokenProvider.CreateSharedAccessSignatureTokenProvider("sas");
        });

        #endregion
    }

    void RegisterTokenProviderUsingNamespaceManagerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider-namespace-manager-settings

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespaceManagers = transport.NamespaceManagers();
        namespaceManagers.NamespaceManagerSettingsFactory(s =>
        {
            return new NamespaceManagerSettings
            {
                TokenProvider = Microsoft.ServiceBus.TokenProvider.CreateSharedAccessSignatureTokenProvider("sas")
            };
        });

        #endregion
    }

    void RegisterTokenProviderUsingMessagingFactorySettings(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider-messaging-factory-settings

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var messagingFactories = transport.MessagingFactories();
        messagingFactories.MessagingFactorySettingsFactory(s =>
        {
            return new MessagingFactorySettings
            {
                TokenProvider = Microsoft.ServiceBus.TokenProvider.CreateSharedAccessSignatureTokenProvider("sas")
            };
        });

        #endregion
    }
}