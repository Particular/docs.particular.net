---
title: Custom Token Provider
summary: Configuring Azure Service Bus transport to use a custom token provider for authentication
component: ASB
versions: '[7,)'
reviewed: 2021-01-04
redirects:
 - nservicebus/azure-service-bus/token-provider
 - transports/azure-service-bus/token-provider
---

include: legacy-asb-warning

By default, the Azure Service Bus transport authenticates to the service using the information embedded in the connection string. It's also possible to authenticate using any of the authentication mechanisms supported by [Azure Service Bus](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-authentication-and-authorization).

This is useful when, for example, delegating authentication and authorization to a Federated Identity infrastructure such as Active Directory Access Control Service or [Active Directory Federation Services](https://technet.microsoft.com/en-us/windows).

Changing the authentication mechanism is done using the [Azure Service Bus SDK's TokenProvider class](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.tokenprovider). The Azure Service Bus SDK requires an instance of this class at 2 different levels.

 * `NamespaceManager`: requires a `TokenProvider` that issues tokens with manage rights on the namespace. Note that this is only needed if queue creation is enabled, so that it can list, create and update entities in the namespace.
 * `MessagingFactory`: requires a `TokenProvider` that issues tokens with at least send or receive rights on the entities used by the endpoint.

By default the transport configures the token provider at the level of the `NamespaceManager` using the connectionstring information and reuses this instance for the `MessagingFactory`.


## Replacing the NamespaceManager Token Provider

The instance at the `NamespaceManager` level can be replaced using the `NamespaceManagers().TokenProvider()` configuration API.

Snippet: asb-register-token-provider

Or alternatively using the `NamespaceManagers().NamespaceManagerSettingsFactory()` configuration API that allows to override the `NamespaceManagerSettings`.

Snippet: asb-register-token-provider-namespace-manager-settings


## Replacing the MessagingFactory Token Provider

If the `MessagingFactory` requires different tokens for authentication then the `NamespaceManager`, its `TokenProvider` can be replaced using the `MessagingFactories().MessagingFactorySettingsFactory()` configuration API that allows to override the `MessagingFactorySettings`.

Snippet: asb-register-token-provider-messaging-factory-settings