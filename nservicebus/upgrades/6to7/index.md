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

The Configuration APIs `IProvideConfiguration`, `IConfigurationSource` and `CustomConfigurationSource` have been deprecated. The equivalent code based API can be used instead.


### [Audit](/nservicebus/operations/auditing.md)

Configuring Audit via the following APIs have been deprecated:

 * `IProvideConfiguration<AuditConfig>`
 * `AuditConfig` in an app.config `configSections`
 * Returning a `AuditConfig` from a `IConfigurationSource`
 * `HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceBus\AuditQueue` registry key
 

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
 * `HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceBus\ErrorQueue` registry key

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


### StorageType

`NServiceBus.Persistence.StorageType` has been moved to the root `NServiceBus` namespace to prevent requiring additional using statements when specifying individual storage types explicitly.


## Assembly scanning


### Mismatched assemblies

64-bit assemblies are no longer silently excluded from scanning when running in a x86 process. Instead startup will fail with a `System.BadImageFormatException`. Use the [exclude API](/nservicebus/hosting/assembly-scanning.md#assemblies-to-scan) to exclude the assembly and avoid the exception.


### AppDomain scanning

AppDomain assemblies are now scanned by default. Use the [ScanAppDomainAssemblies API](/nservicebus/hosting/assembly-scanning.md#appdomain-assemblies) to disable AppDomain scanning.


## Legacy .Retries message receiver

The [.Retries message receiver](/nservicebus/recoverability/configure-delayed-retries.md?version=core_6#custom-retry-policy-legacy-retries-message-receiver) added to assist in migrating from Version 5 to Version 6 has been removed. The API to disable it has also been removed.


## MSMQ

The [MSMQ transport](/transports/msmq) is no longer part of the NServiceBus NuGet package. It has been moved into a separate package, [NServiceBus.Transport.Msmq](https://www.nuget.org/packages/NServiceBus.Transport.Msmq/).

### Namespace changes
The `MsmqPersistence` class and its configuration API, `SubscriptionQueue()`, have been moved from `NServiceBus.Persistence.Legacy` namespace to `NServiceBus`. 

MSMQ persistence was originally put into the legacy namespace because of its limited capabilities in scale-out scenarios with the distributor. Sender-side distribution changes this and makes MSMQ persistence a viable persistence mechanism when scaling out MSMQ. It was therefore moved from the legacy namespace and back into `NServiceBus`.


## Default transport

There is no longer a default transport, so an exception will be thrown if an endpoint is created or started without configuring a transport.

In Versions 6 and below the default transport was [MSMQ](/transports/msmq/). To use MSMQ in Versions 7 and above, reference [NServiceBus.Transport.Msmq](https://www.nuget.org/packages/NServiceBus.Transport.Msmq/) and configure with:

snippet: 6to7-UseMsmqTransport


## Message Property Encryption

The [Message Property Encryption feature](/nservicebus/security/property-encryption.md) has been moved from the NServiceBus package. It is now available as a separate NuGet package, [NServiceBus.Encryption.MessageProperty](https://www.nuget.org/packages/NServiceBus.Encryption.MessageProperty/).

See the [NServiceBus.Encryption.MessageProperty upgrade guide](/nservicebus/upgrades/externalize-encryption.md) for more details.


## JSON serialization

The [JSON serializer](/nservicebus/serialization/json.md) has been removed from the NServiceBus package. Use the external JSON serializer available as a separate NuGet package, `NServiceBus.Newtonsoft.Json`.

See the [Json.NET Serializer](/nservicebus/serialization/newtonsoft.md) for more details, including its [compatibility](/nservicebus/serialization/newtonsoft.md#compatibility-with-the-core-json-serializer) with the previously available JSON serializer.


## Custom Correlation ID

Setting a custom [correlation ID](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-correlationid) is considered dangerous. Therefore, the `SendOptions.SetCorrelationId` and `SendOptions.GetCorrelationId` APIs have been removed.


## Accessing Conversation ID

In NServiceBus Version 6, the `Conversation Id` header on outgoing messages was set within the `IOutgoingPhysicalMessageContext` pipeline stage. In Versions 7 and above, the `Conversation Id` header will be set as part of the `IOutgoingLogicalMessageContext` stage.


## ConfigurationErrorsException

Exceptions of type `Exception` are now thrown instead of `ConfigurationErrorsException`. Any try-catch statements catching `ConfigurationErrorsException` should be updated to catch `Exception` instead.


## Licensing


### Machine-wide license locations

License files are now stored on the local file system so that they can accessed by all endpoints running on the machine. By default, endpoints will check the following locations for a `license.xml` file:

 * `{Environment.SpecialFolder.LocalApplicationData}\ParticularSoftware`
 * `{Environment.SpecialFolder.CommonApplicationData}\ParticularSoftware`


### Application-specific license location

Licenses can be shipped along with an endpoint's artifacts. By default, endpoints will look for a `license.xml` in the applications base directory (`AppDomain.CurrentDomain.BaseDirectory`).

WARNING: The `{AppDomain.CurrentDomain.BaseDirectory}\License\License.xml` path will no longer be checked.


### Registry-based license locations

When running on the .NET Framework, endpoints will [continue to search the registry locations](/nservicebus/licensing/) for a suitable license.

When running on .NET Core, endpoints **will not search the registry**, even when running on Windows.


## Connection Strings


### Named connection strings

Configuring a transport's connection using `.ConnectionStringName(name)` has been removed. To continue to retrieve the connection string by the named value in the configuration, first retrieve the connection string and then pass it to the `.ConnectionString(value)` configuration.


### Implicit "NServiceBus/Transport" connection string use

When running on .NET Core, a connection string named `NServiceBus/Transport` will **no longer be automatically detected**. The connection string value must be explicitly configured using `.ConnectionString(value)`.

When running on the .NET Framework, the `NServiceBus/Transport` connection string will continue to function as per previous version, however a warning will be logged indicating that it should be explicitly configured instead.

snippet: 6to7ConnectionStrings


## Installers

NServiceBus Version 7 will only run installers when explicitly enabled via the `endpointConfiguration.EnableInstallers()` API. In previous versions, installers are automatically run when starting the endpoint with a debugger attached, this behavior has been removed in Version 7. Therefore, the `endpointConfiguration.DisableInstallers()` API has been obsoleted and is no longer required.


## DistributionStrategy

The `string SelectReceiver(string[] receiverAddresses)` signature has been removed from the `DistributionStrategy` base class. When writing a custom distribution strategy, implement the `string SelectDestination(DistributionContext context)` method instead which provides additional information usable for routing decisions. Receiver addresses can still be accessed via the `context.ReceiverAddresses` property.
