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


#### ProvideConfiguration

`IProvideConfiguration<AuditConfig>` is deprecated.

snippet: 6to7AuditProvideConfiguration


#### Using XML


Using `section name="AuditConfig"` in app.config is deprecated.

snippet: 6to7configureAuditUsingXml

It can be replaced with a combination of a custom appSettings and then using the code API.

snippet: 6to7configureAuditUsingXmlAppSettings


snippet: 6to7configureAuditUsingXmlReadAppSettings


### [Logging](/nservicebus/logging/)


#### ProvideConfiguration

`IProvideConfiguration<Logging>` is deprecated.

snippet: 6to7LoggingThresholdFromIProvideConfiguration


#### Using XML


Using `section name="Logging"` in app.config is deprecated.

snippet: 6to7OverrideLoggingDefaultsInAppConfig

It can be replaced with a combination of a custom appSettings and then using the code API.

snippet: 6to7OverrideLoggingDefaultsInAppSettings

snippet: 6to7OverrideLoggingDefaultsReadAppSettings


### [Error Queue](/nservicebus/recoverability/configure-error-handling.md)


#### ProvideConfiguration

`IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>` is deprecated.

snippet: 6to7ErrorQueueConfigurationProvider



#### Using XML

snippet: 6to7configureErrorQueueViaXml

It can be replaced with a combination of a custom appSettings and then using the code API.

snippet: 6to7configureErrorQueueViaAppSettings

snippet: 6to7configureErrorQueueReadAppSettings


#### IConfigurationSource

Returning a `MessageForwardingInCaseOfFaultConfig` from a `IConfigurationSource` is deprecated.

snippet: 6to7ErrorQueueConfigurationSource

It can be replaced with a combination of a custom appSettings and then using the code API.

snippet: 6to7UseCustomConfigurationSourceForErrorQueueConfig

snippet: 6to7UseCustomConfigurationSourceForErrorQueueConfigNew


### [Endpoint Mappings](/nservicebus/messaging/routing.md)

Using `section name="UnicastBusConfig"` and `MessageEndpointMappings` in app.config is deprecated.

snippet: 6to7endpoint-mapping-appconfig

It can be replaced with [Command routing](/nservicebus/messaging/routing-extensibility.md#routing-apis-command-routing) and [Event routing](/nservicebus/messaging/routing-extensibility.md#routing-apis-event-routing)

snippet: 6to7endpoint-mapping-Routing-Logical

snippet: 6to7endpoint-mapping-Routing-RegisterPublisher


include: dependencies
