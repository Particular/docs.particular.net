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


include: dependencies
