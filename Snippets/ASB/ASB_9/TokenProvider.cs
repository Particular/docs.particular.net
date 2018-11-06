using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;

class TokenProviderUsage
{
    void RegisterTokenProvider(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var managers = transport.NamespaceManagers();
        managers.TokenProvider(
            factory: s =>
            {
                return TokenProvider.CreateSharedAccessSignatureTokenProvider("sas");
            });

        #endregion
    }

    void RegisterTokenProviderUsingNamespaceManagerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider-namespace-manager-settings

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var managers = transport.NamespaceManagers();
        managers.NamespaceManagerSettingsFactory(
            factory: s =>
            {
                return new NamespaceManagerSettings
                {
                    TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("sas")
                };
            });

        #endregion
    }

    void RegisterTokenProviderUsingMessagingFactorySettings(EndpointConfiguration endpointConfiguration)
    {
        #region asb-register-token-provider-messaging-factory-settings

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var factories = transport.MessagingFactories();
        factories.MessagingFactorySettingsFactory(
            factory: s =>
            {
                return new MessagingFactorySettings
                {
                    TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("sas")
                };
            });

        #endregion
    }
}