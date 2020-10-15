---
title: Upgrade Version 6 to 7
summary: Instructions on how to upgrade NServiceBus from version 6 to version 7.
component: Core
reviewed: 2020-03-23
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


include: upgrade-major


## Configuration

The configuration APIs `IProvideConfiguration`, `IConfigurationSource`, and `CustomConfigurationSource` have been deprecated. The equivalent code-based API are shown in the remainder of this section.


### [Auditing](/nservicebus/operations/auditing.md)

Configuring auditing via the following APIs has been deprecated:

 * `IProvideConfiguration<AuditConfig>`
 * `AuditConfig` in an app.config `configSections`
 * Returning an `AuditConfig` from an `IConfigurationSource`
 * `HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceBus\AuditQueue` registry key


Instead use one of the following methods:

#### Configure with the code API

snippet: 6to7AuditCode


#### Read from appSettings and configure with the code API

snippet: 6to7AuditAppSettings

snippet: 6to7AuditReadAppSettings


### [Logging](/nservicebus/logging/)

Configuring logging via the following APIs has been deprecated:

 * `IProvideConfiguration<Logging>`
 * `Logging` in an app.config `configSections`


Instead use one of the following methods:


#### Configure with the code API

snippet: 6to7LoggingCode


#### Read from appSettings and configure with the code API

snippet: 6to7LoggingAppSettings

snippet: 6to7LoggingReadAppSettings


### [Error queue](/nservicebus/recoverability/configure-error-handling.md)

Configuring the error queue via the following APIs has been deprecated:

 * `IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>`
 * `MessageForwardingInCaseOfFaultConfig` in an app.config `configSections`
 * Returning a `MessageForwardingInCaseOfFaultConfig` from an `IConfigurationSource`
 * `HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceBus\ErrorQueue` registry key

Instead use one of the following methods:


#### Configure with the code API

snippet: 6to7ErrorCode


#### Read from appSettings and configure with the code API

snippet: 6to7ErrorAppSettings

snippet: 6to7ErrorReadAppSettings


### [Endpoint mappings](/nservicebus/messaging/routing.md)

Configuring endpoint mappings via the following APIs has been deprecated:

 * `IProvideConfiguration<UnicastBusConfig>`
 * `UnicastBusConfig/MessageEndpointMappings` in an app.config `configSections`
 * Returning a `UnicastBusConfig` from an `IConfigurationSource`

NOTE: **MSMQ**: [Machine names are set via the MSMQ transport routing configuration](/transports/msmq/routing.md).

It can be replaced with a combination of the following methods:


#### [Command routing](/nservicebus/messaging/routing-extensibility.md#routing-apis-command-routing)

snippet: 6to7endpoint-mapping-Routing-Logical


#### [Event routing](/nservicebus/messaging/routing-extensibility.md#routing-apis-event-routing)

snippet: 6to7endpoint-mapping-Routing-RegisterPublisher


## Renamed APIs

### Access to settings

The `GetSettings` extension method has been moved from `NServiceBus.Configuration.AdvanceExtensibility` to the `NServiceBus.Configuration.AdvancedExtensibility` namespace.


###  ContextBag extensions

The `RemoveDeliveryConstaint` extension method has been renamed to `RemoveDeliveryConstraint`.


### IncomingMessage extensions

The `GetMesssageIntent` extension method has been renamed to `GetMessageIntent`.


## Pipeline configuration

`RegisterStep.IsEnabled` has been removed. Instead of overriding this method to disable registration, users should instead not register the steps in the pipeline at all.


### StorageType

`NServiceBus.Persistence.StorageType` has been moved to the root `NServiceBus` namespace to prevent requiring additional using statements when specifying individual storage types explicitly.


## Assembly scanning


### Mismatched assemblies

64-bit assemblies are no longer silently excluded from scanning when running in an x86 process. Instead, startup will fail with a [BadImageFormatException](https://msdn.microsoft.com/en-us/library/system.badimageformatexception.aspx). Use the [exclude API](/nservicebus/hosting/assembly-scanning.md#assemblies-to-scan) to exclude the assembly and avoid the exception.


### AppDomain scanning

AppDomain assemblies are now scanned by default. Use the [ScanAppDomainAssemblies API](/nservicebus/hosting/assembly-scanning.md#appdomain-assemblies) to disable AppDomain scanning.


### Unobtrusive mode messages

[Unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md) message types will no longer appear in the list of scanned types. These message types are now loaded dynamically as messages arrive.


## Legacy .Retries message receiver

The [.Retries message receiver](/nservicebus/recoverability/configure-delayed-retries.md?version=core_6#custom-retry-policy-legacy-retries-message-receiver), which was added to assist in migrating from version 5 to version 6, has been removed. The API to disable it has also been removed.


## MSMQ

The [MSMQ transport](/transports/msmq) is no longer part of the NServiceBus NuGet package. It has been moved into a separate package, [NServiceBus.Transport.Msmq](https://www.nuget.org/packages/NServiceBus.Transport.Msmq/).

### Provision of PowerShell scripts

Two new scripts, `CreateQueues.ps1` and `DeleteQueues.ps1`, have been added to the NuGet package to facilitate the creation of queues for endpoints during deployment. These scripts are copied to a subfolder called `NServiceBus.Transport.Msmq` in the output folder of any project referencing it. Browse to the output folder to locate the scripts. For example, `bin\Debug\net461\NServiceBus.Transport.Msmq`.

A new API, [DisableInstaller](/transports/msmq/transportconfig.md?version=msmqtransport_1#receiving-algorithm-disableinstaller), can now be used to disable the auto-creation of queues during startup.


### New transport configuration API

Passing in the [connection string](/transports/msmq/connection-strings.md) when configuring transports is no longer supported. If the connection string is passed, the following exception will be thrown at endpoint start-up:

> System.Exception : Passing in MSMQ settings such as DeadLetterQueue, Journaling etc via a connection string is no longer supported.  Use code-level API.

New APIs have been added for each of the settings, namely, DisableDeadLetterQueueing, DisableConnectionCachingForSends, UseNonTransactionalQueues, EnableJournaling and TimeToReachQueue. See the [transport configuration documentation](/transports/msmq/transportconfig.md) for more details on the usage.


### MSMQ subscription storage

The default queue for the subscription storage has been switched from `NServiceBus.Subscriptions` to `[EndpointName].Subscriptions` if the subscription queue has not been explicitly configured.

However, if a subscription storage queue is not provided during configuration time and if the endpoint detects a local queue in the server called `NServiceBus.Subscriptions`, an exception will be thrown to prevent potential loss of messages. To prevent this, [move the subscription messages to the new queue](/nservicebus/upgrades/6to7/moving-msmq-subscriptions.md).


### Namespace changes

The `MsmqPersistence` class and its configuration API, `SubscriptionQueue()`, have been moved from the `NServiceBus.Persistence.Legacy` namespace to `NServiceBus`.

MSMQ persistence was originally put into the legacy namespace because of its limited capabilities in scale-out scenarios with the distributor. Sender-side distribution makes MSMQ persistence a viable persistence mechanism when scaling out MSMQ. It was therefore moved from the legacy namespace and back into `NServiceBus`.


## Default transport

There is no longer a default transport; an exception will be thrown if an endpoint is created or started without configuring a transport.

In NServiceBus version 6 and below, the default transport was [MSMQ](/transports/msmq/). To use MSMQ in version 7 and above, reference [NServiceBus.Transport.Msmq](https://www.nuget.org/packages/NServiceBus.Transport.Msmq/) and configure with:

snippet: 6to7-UseMsmqTransport


## Message property encryption

The [message property encryption feature](/nservicebus/security/property-encryption.md) has been moved from the NServiceBus package. It is now available as a separate NuGet package, [NServiceBus.Encryption.MessageProperty](https://www.nuget.org/packages/NServiceBus.Encryption.MessageProperty/).

See the [NServiceBus.Encryption.MessageProperty upgrade guide](/nservicebus/upgrades/externalize-encryption.md) for more details.


## JSON serialization

The [JSON serializer](/nservicebus/serialization/json.md) has been removed from the NServiceBus package. Use the external JSON serializer available as a separate NuGet package, `NServiceBus.Newtonsoft.Json`.

See the [Json.NET Serializer](/nservicebus/serialization/newtonsoft.md) for more details, including its [compatibility](/nservicebus/serialization/newtonsoft.md#compatibility-with-the-core-json-serializer) with the previously available JSON serializer.


## Custom correlation ID

Setting a custom [correlation ID](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-correlationid) is considered dangerous. Therefore, the `SendOptions.SetCorrelationId` and `SendOptions.GetCorrelationId` APIs have been removed.


## Accessing conversation ID

In NServiceBus version 6, the `Conversation Id` header on outgoing messages was set within the `IOutgoingPhysicalMessageContext` pipeline stage. In version 7 and above, the `Conversation Id` header will be set as part of [the `IOutgoingLogicalMessageContext` stage](/nservicebus/pipeline/steps-stages-connectors.md#stages-outgoing-pipeline-stages).


## ConfigurationErrorsException

Exceptions of type `Exception` are now thrown instead of `ConfigurationErrorsException`. Any try-catch statements catching `ConfigurationErrorsException` should be updated to catch `Exception` instead.


## Licensing


### Machine-wide license locations

License files are now stored on the local file system so that they can accessed by all endpoints running on the machine. By default, endpoints will check the following locations for a `license.xml` file:

 * `{SpecialFolder.LocalApplicationData}\ParticularSoftware`
 * `{SpecialFolder.CommonApplicationData}\ParticularSoftware`


### Application-specific license location

Licenses can be shipped along with an endpoint's artifacts. By default, endpoints will look for a `license.xml` in the applications base directory (`AppDomain.CurrentDomain.BaseDirectory`).

WARNING: The `{AppDomain.CurrentDomain.BaseDirectory}\License\License.xml` path will no longer be checked.


### Registry-based license locations

When running on the .NET Framework, endpoints will [continue to search the registry locations](/nservicebus/licensing/) for a suitable license.

When running on .NET Core, endpoints **will not search the registry**, even when running on Windows.


## Connection strings


### Named connection strings

When running on .NET Core, it is no longer possible to configure a transport's connection using `.ConnectionStringName(name)`. To continue to retrieve the connection string by the named value in the configuration, first retrieve the connection string and then pass it to the `.ConnectionString(value)` configuration.

When running on the .NET Framework, `.ConnectionStringName(name)` will continue to work, but the API has been marked with an obsolete warning suggesting to move to the `.ConnectionString(value)` API.


### Implicit "NServiceBus/Transport" connection string use

When running on .NET Core, a connection string named `NServiceBus/Transport` will **no longer be detected automatically**. The connection string value must be configured explicitly using `.ConnectionString(value)`.

When running on the .NET Framework, the `NServiceBus/Transport` connection string will continue to function as per previous versions, however a warning will be logged indicating that it should be explicitly configured instead.

snippet: 6to7ConnectionStrings


## Installers

NServiceBus version 7 will run installers only when explicitly enabled via the `endpointConfiguration.EnableInstallers()` API. In previous versions, installers are automatically run when starting the endpoint with a debugger attached; this behavior has been removed in version 7. Therefore, the `endpointConfiguration.DisableInstallers()` API is obsolete and no longer required.


## DistributionStrategy

The `string SelectReceiver(string[] receiverAddresses)` signature has been removed from the `DistributionStrategy` base class. When writing a custom distribution strategy, implement the `string SelectDestination(DistributionContext context)` method instead which provides additional information usable for routing decisions. Receiver addresses can still be accessed via the `context.ReceiverAddresses` property.


## HandleCurrentMessageLater

The `IMessageHandlerContext.HandleCurrentMessageLater()` method has been deprecated.

To handle the current message later and abort the current processing attempt, throw an exception in the message handler and let [recoverability](/nservicebus/recoverability) reschedule the message. Note the following restrictions:

 * Retries are enabled only when the transport is configured to use transactions (i.e. anything other than [`TransportTransactionMode.None`](/transports/transactions.md#transactions-unreliable-transactions-disabled).
 * When throwing an exception, the current transaction will be rolled back, causing outgoing messages to be discarded.
 * The retry attempts and delays depend on the specific configuration.
 * Depending on the transport's transaction behavior, the message will reappear at the front or at the back of the queue.

To complete processing of the current message without invoking additional handlers and reprocess it later, send a copy of the current message via `IMessageHandlerContext.SendLocal(...)`. Note the following restrictions:

 * Reusing the incoming message instance is possible, however it does not copy the headers of the incoming message. Headers need to be manually set on the outgoing message via the [outgoing headers API](/nservicebus/messaging/header-manipulation.md#writing-outgoing-headers).
 * A delay can be added using the send options. For more options see the [delayed delivery](/nservicebus/messaging/delayed-delivery.md) section.
 * The sent message will be added at the back of the queue.


## Default critical error behavior

In NServiceBus version 6 and below, the default behavior was to stop the endpoint when critical errors occur. In version 7 and above, the default behavior is to keep the endpoint running to allow infrastructure (i.e. transports, persisters, etc.) to try to recover from the failure condition. One example is the queuing system being unavailable or not being able to connect to the database.

See the [critical errors documentation](/nservicebus/hosting/critical-errors.md) for details on how to customize this behavior.


## Startup diagnostics written to disk

When endpoints start, a diagnostics file is written to disk in a subfolder called `.diagnostics`. See the [startup diagnostics documentation](/nservicebus/hosting/startup-diagnostics.md) for more details.

## Routing for send-only endpoints

Routing messages to the local endpoint or local instance is no longer allowed for send-only endpoints, since they are not able to receive messages. When detected, the following exception is thrown:

`System.InvalidOperationException: Cannot route to instances of this endpoint since it's configured to be in send-only mode.`

## Source Link

As of NServiceBus version 7, all packages support [Source Link](https://github.com/dotnet/designs/blob/master/accepted/diagnostics/source-link.md), a developer productivity feature that allows debugging into NServiceBus code by downloading the source directly from GitHub.

There is currently a bug with Visual Studio 2017 [SDK-style projects](https://github.com/dotnet/sdk/issues/1458) that prevents Source Link from working when the project targets the .NET Framework. A [workaround](https://github.com/dotnet/sdk/issues/1458#issuecomment-362685678) for the bug is to add the [SourceLink.Copy.PdbFiles NuGet package](https://www.nuget.org/packages/SourceLink.Copy.PdbFiles) to the project.
