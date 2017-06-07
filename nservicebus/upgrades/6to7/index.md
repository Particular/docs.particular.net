---
title: Upgrade Version 6 to 7
summary: Instructions on how to upgrade NServiceBus Version 6 to 7.
component: Core
reviewed: 2017-05-23
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


include: upgrade-major

## Config

The Configuration APIs `IProvideConfiguration`, `IConfigurationSource` and `CustomConfigurationSource` have been deprecated. Code based API can be used instead.


### [Audit](/nservicebus/operations/auditing.md)

Configuring Audit via the following APIs have been deprecated:

 * `IProvideConfiguration<AuditConfig>`
 * `AuditConfig` in an app.config `configSections`
 * Returning a `AuditConfig` from a `IConfigurationSource`

Instead use one of the following:


#### Configure by the code API

snippet: 6to7AuditCode


#### Read from an AppSettings and configure by the code API

snippet: 6to7AuditAppSettings

snippet: 6to7AuditReadAppSettings


### [Logging](/nservicebus/logging/)

Configuring Logging via the following APIs have been deprecated:

 * `IProvideConfiguration<Logging>`
 * `Logging` in an app.config `configSections`


Instead use one of the following:


#### Configure by the code API

snippet: 6to7LoggingCode


#### Read from an AppSettings and configure by the code API

snippet: 6to7LoggingAppSettings

snippet: 6to7LoggingReadAppSettings


### [Error Queue](/nservicebus/recoverability/configure-error-handling.md)

Configuring the error queue via the following APIs have been deprecated:

 * `IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>`
 * `MessageForwardingInCaseOfFaultConfig` in an app.config `configSections`
 * Returning a `MessageForwardingInCaseOfFaultConfig` from a `IConfigurationSource`

Instead use one of the following:

#### Configure by the code API

snippet: 6to7ErrorCode


#### Read from an AppSettings and configure by the code API

snippet: 6to7ErrorAppSettings

snippet: 6to7ErrorReadAppSettings


### [Endpoint Mappings](/nservicebus/messaging/routing.md)

Configuring the error queue via the following APIs have been deprecated:

 * `IProvideConfiguration<UnicastBusConfig>`
 * `UnicastBusConfig` in an app.config `configSections`
 * Returning a `UnicastBusConfig` from a `IConfigurationSource`

It can be replaced with a combination of the following


#### [Command routing](/nservicebus/messaging/routing-extensibility.md#routing-apis-command-routing) 

snippet: 6to7endpoint-mapping-Routing-Logical


#### [Event routing](/nservicebus/messaging/routing-extensibility.md#routing-apis-event-routing)

snippet: 6to7endpoint-mapping-Routing-RegisterPublisher


## Renamed APIs


### Access to settings

The `GetSettings` extension method has been moved from `NServiceBus.Configuration.AdvanceExtensibility` to the `NServiceBus.Configuration.AdvancedExtensibility` namespace. More details on advanced access to settings can be found [here](/nservicebus/pipeline/features.md#feature-settings-endpointconfiguration).


###  ContextBag extensions

The `RemoveDeliveryConstaint` extension method has been renamed to `RemoveDeliveryConstraint`.


### IncomingMessage extensions

The `GetMesssageIntent` extension method has been renamed to `GetMessageIntent`.


## Assembly scanning

64-bit assemblies are no longer silently excluded from scanning when running in a x86 process. Instead startup will fail with a `System.BadImageFormatException`. Use the [exclude API](/nservicebus/hosting/assembly-scanning.md#assemblies-to-scan) to exclude the assembly and avoid the exception. 


## Legacy .Retries message receiver

The [.Retries message receiver](/nservicebus/recoverability/configure-delayed-retries.md?version=core_6#custom-retry-policy-legacy-retries-message-receiver) added to assist in migrating from Version 5 to Version 6 has been removed. The API to disable it has also been removed.


## MSMQ

The [MSMQ transport](/nservicebus/msmq) is no longer part of the NServiceBus NuGet package. It has been moved into a separate package, [NServiceBus.Transport.Msmq](https://www.nuget.org/packages/NServiceBus.Transport.Msmq/).


## Default transport

There is no longer a default transport, so an exception will be thrown if an endpoint is created or started without configuring a transport.


## Message Property Encryption

The [Message Property Encryption feature](/nservicebus/security/property-encryption.md) has been moved from the NServiceBus package. It is now available as a separate NuGet package, [NServiceBus.Encryption.MessageProperty](https://www.nuget.org/packages/NServiceBus.Encryption.MessageProperty/).

See the [NServiceBus.Encryption.MessageProperty upgrade guide](/nservicebus/upgrades/externalize-encryption.md) for more details.
